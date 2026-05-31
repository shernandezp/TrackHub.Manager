using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Write, PrincipalTypes = "User,ServiceClient")]
public readonly record struct SynchronizeOperatorDevicesCommand(
    Guid AccountId,
    Guid OperatorId,
    IReadOnlyCollection<DeviceDto> Devices,
    string CorrelationId,
    string TriggerType = "AUTOMATIC",
    bool? AutoAssignNewDevices = null) : IRequest<OperatorSyncRunVm>;

public class SynchronizeOperatorDevicesCommandHandler(
    IDeviceWriter deviceWriter,
    IDeviceReader deviceReader,
    ITransporterWriter transporterWriter,
    ITransporterDeviceAssignmentWriter assignmentWriter,
    IOperatorSyncRunWriter syncWriter,
    IOperatorWriter operatorWriter,
    IAlertEventWriter alertWriter,
    ILogger<SynchronizeOperatorDevicesCommandHandler> logger)
    : IRequestHandler<SynchronizeOperatorDevicesCommand, OperatorSyncRunVm>
{
    public async Task<OperatorSyncRunVm> Handle(SynchronizeOperatorDevicesCommand request, CancellationToken cancellationToken)
    {
        var startedAt = DateTimeOffset.UtcNow;
        if (request.Devices.Any(d => d.AccountId != request.AccountId || d.OperatorId != request.OperatorId))
        {
            throw new ForbiddenAccessException();
        }

        var existing = await deviceReader.GetDevicesByOperatorAsync(request.OperatorId, cancellationToken);
        var existingByIdentifier = existing
            .GroupBy(d => d.Identifier)
            .ToDictionary(g => g.Key, g => g.First());

        var incomingIdentifiers = new HashSet<int>();
        var newlyAdded = new List<DeviceDto>();
        var newlyAddedDevices = new List<(DeviceDto Incoming, DeviceVm Device)>();
        int added = 0, updated = 0, ignored = 0;
        foreach (var incoming in request.Devices)
        {
            var upserted = await deviceWriter.UpsertSynchronizedDeviceAsync(incoming, cancellationToken);
            incomingIdentifiers.Add(incoming.Identifier);

            if (!existingByIdentifier.ContainsKey(incoming.Identifier))
            {
                added++;
                newlyAdded.Add(incoming);
                newlyAddedDevices.Add((incoming, upserted));
            }
            else
            {
                updated++;
            }

            if (upserted.DetectedStatus == DetectedStatus.Ignored)
                ignored++;
        }

        var removed = existingByIdentifier
            .Where(kvp => !incomingIdentifiers.Contains(kvp.Key))
            .Select(kvp => kvp.Value)
            .ToList();

        if (request.AutoAssignNewDevices ?? true)
        {
            await AutoAssignNewDevicesAsync(request.AccountId, newlyAddedDevices, cancellationToken);
        }

        var duplicates = new List<(string Serial, Guid OtherOperatorId)>();
        if (newlyAdded.Count > 0)
        {
            var dupRows = await deviceReader.FindDuplicateSerialsAsync(
                request.AccountId,
                request.OperatorId,
                newlyAdded.Select(d => d.Serial).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(),
                cancellationToken);
            duplicates = dupRows.Select(d => (d.Serial, d.OperatorId)).ToList();
        }

        await EmitDeviceAlertsAsync(request, newlyAdded, removed, duplicates, cancellationToken);

        var finishedAt = DateTimeOffset.UtcNow;
        var triggerType = ResolveTriggerType(request.TriggerType);
        var run = await syncWriter.RecordAsync(new OperatorSyncRunDto(
            request.AccountId, request.OperatorId, triggerType, OperatorSyncResult.Succeeded,
            startedAt, finishedAt, request.Devices.Count, added, updated, removed.Count, ignored, 0, 0, 0,
            null, null, request.CorrelationId), cancellationToken);

        await operatorWriter.UpdateSyncSummaryAsync(
            request.OperatorId, success: true, finishedAt, triggerType,
            deviceSync: true, positionSync: false, null, null, cancellationToken);

        return run;
    }

    private async Task EmitDeviceAlertsAsync(
        SynchronizeOperatorDevicesCommand request,
        IReadOnlyCollection<DeviceDto> newlyAdded,
        IReadOnlyCollection<DeviceVm> removed,
        IReadOnlyCollection<(string Serial, Guid OtherOperatorId)> duplicates,
        CancellationToken cancellationToken)
    {
        try
        {
            foreach (var device in newlyAdded)
            {
                await alertWriter.RecordAlertEventAsync(new AlertEventDto(
                    request.AccountId,
                    EventType: "GpsDeviceDetected",
                    Severity: "Info",
                    SourceModule: "GpsIntegration",
                    ResourceType: "SynchronizedDevice",
                    ResourceId: device.Identifier.ToString(),
                    Status: "Open",
                    PayloadJson: JsonSerializer.Serialize(new { request.OperatorId, device.Identifier, device.Serial, request.CorrelationId }),
                    DeduplicationKey: $"gps-device-detected:{device.Identifier}:{request.OperatorId:N}"),
                    cancellationToken);
            }
            foreach (var device in removed)
            {
                await alertWriter.RecordAlertEventAsync(new AlertEventDto(
                    request.AccountId,
                    EventType: "GpsDeviceRemoved",
                    Severity: "Warning",
                    SourceModule: "GpsIntegration",
                    ResourceType: "SynchronizedDevice",
                    ResourceId: device.Identifier.ToString(),
                    Status: "Open",
                    PayloadJson: JsonSerializer.Serialize(new { request.OperatorId, device.Identifier, device.Serial, request.CorrelationId }),
                    DeduplicationKey: $"gps-device-removed:{device.Identifier}:{request.OperatorId:N}"),
                    cancellationToken);
            }
            foreach (var (serial, otherOperatorId) in duplicates)
            {
                await alertWriter.RecordAlertEventAsync(new AlertEventDto(
                    request.AccountId,
                    EventType: "GpsDuplicateDeviceIdentifier",
                    Severity: "Warning",
                    SourceModule: "GpsIntegration",
                    ResourceType: "SynchronizedDevice",
                    ResourceId: serial,
                    Status: "Open",
                    PayloadJson: JsonSerializer.Serialize(new { Serial = serial, ReportedBy = request.OperatorId, AlsoOwnedBy = otherOperatorId }),
                    DeduplicationKey: $"gps-duplicate-device:{serial}:{request.AccountId:N}"),
                    cancellationToken);
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex,
                "Failed to emit device sync alerts for operator {OperatorId}; the sync run itself is still recorded.",
                request.OperatorId);
        }
    }

    private async Task AutoAssignNewDevicesAsync(
        Guid accountId,
        IReadOnlyCollection<(DeviceDto Incoming, DeviceVm Device)> devices,
        CancellationToken cancellationToken)
    {
        foreach (var (incoming, device) in devices)
        {
            var transporter = await transporterWriter.CreateTransporterAsync(
                new TransporterDto(
                    ResolveTransporterName(incoming),
                    ResolveTransporterTypeId(incoming.DeviceTypeId),
                    accountId),
                cancellationToken);

            await assignmentWriter.AssignAsync(
                new TransporterDeviceAssignmentDto(
                    accountId,
                    transporter.TransporterId,
                    device.DeviceId,
                    Priority: 0,
                    IsPrimary: true,
                    AssignmentReason: "Initial provider sync"),
                cancellationToken);
        }
    }

    private static string ResolveTransporterName(DeviceDto device)
        => FirstNonEmpty(device.ProviderDisplayName, device.Name, device.Serial, $"Device {device.Identifier}");

    private static string FirstNonEmpty(params string?[] values)
        => values.Select(v => v?.Trim()).FirstOrDefault(v => !string.IsNullOrWhiteSpace(v))!;

    private static short ResolveTransporterTypeId(short deviceTypeId)
        => (short)(Enum.IsDefined(typeof(Common.Domain.Enums.DeviceType), (int)deviceTypeId)
            ? (Common.Domain.Enums.DeviceType)deviceTypeId switch
            {
                Common.Domain.Enums.DeviceType.Aviation => Common.Domain.Enums.TransporterType.Aircraft,
                Common.Domain.Enums.DeviceType.Cycling => Common.Domain.Enums.TransporterType.Bicycle,
                Common.Domain.Enums.DeviceType.Drones => Common.Domain.Enums.TransporterType.Drone,
                Common.Domain.Enums.DeviceType.Marine => Common.Domain.Enums.TransporterType.Boat,
                Common.Domain.Enums.DeviceType.PetTracking => Common.Domain.Enums.TransporterType.Pet,
                Common.Domain.Enums.DeviceType.Phone or Common.Domain.Enums.DeviceType.Fitness or Common.Domain.Enums.DeviceType.Smartwatch or Common.Domain.Enums.DeviceType.Wearable => Common.Domain.Enums.TransporterType.Person,
                Common.Domain.Enums.DeviceType.OBDScanner => Common.Domain.Enums.TransporterType.FleetVehicle,
                _ => Common.Domain.Enums.TransporterType.Asset
            }
            : Common.Domain.Enums.TransporterType.Asset);

    private static SyncTriggerType ResolveTriggerType(string triggerType)
        => Enum.TryParse<SyncTriggerType>(triggerType, ignoreCase: true, out var parsed)
            ? parsed
            : SyncTriggerType.Automatic;
}
