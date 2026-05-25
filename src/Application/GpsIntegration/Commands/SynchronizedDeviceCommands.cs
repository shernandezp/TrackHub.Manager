using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Write, PrincipalTypes = "ServiceClient")]
[RequireFeature(FeatureKeys.GpsIntegration, AllowGlobalServiceClient = false)]
public readonly record struct SynchronizeOperatorDevicesCommand(
    Guid AccountId,
    Guid OperatorId,
    IReadOnlyCollection<DeviceDto> Devices,
    string CorrelationId) : IRequest<OperatorSyncRunVm>;

public class SynchronizeOperatorDevicesCommandHandler(
    IDeviceWriter deviceWriter,
    IDeviceReader deviceReader,
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
        int added = 0, updated = 0, ignored = 0;
        foreach (var incoming in request.Devices)
        {
            var upserted = await deviceWriter.UpsertSynchronizedDeviceAsync(incoming, cancellationToken);
            incomingIdentifiers.Add(incoming.Identifier);

            if (!existingByIdentifier.ContainsKey(incoming.Identifier))
            {
                added++;
                newlyAdded.Add(incoming);
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
        var run = await syncWriter.RecordAsync(new OperatorSyncRunDto(
            request.AccountId, request.OperatorId, SyncTriggerType.Automatic, OperatorSyncResult.Succeeded,
            startedAt, finishedAt, request.Devices.Count, added, updated, removed.Count, ignored, 0, 0, 0,
            null, null, request.CorrelationId), cancellationToken);

        await operatorWriter.UpdateSyncSummaryAsync(
            request.OperatorId, success: true, finishedAt, SyncTriggerType.Automatic,
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
}
