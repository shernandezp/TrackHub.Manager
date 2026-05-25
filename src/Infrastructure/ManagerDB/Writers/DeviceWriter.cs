using Common.Application.Exceptions;
using Common.Application.Interfaces;
using Common.Domain.Enums;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class DeviceWriter(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IDeviceWriter
{
    public async Task<DeviceVm> UpsertSynchronizedDeviceAsync(DeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var accountId = RequireAccountAccess(deviceDto.AccountId);
        var operatorAccountId = await Context.Operators
            .Where(o => o.OperatorId == deviceDto.OperatorId)
            .Select(o => (Guid?)o.AccountId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Entities.Operator), deviceDto.OperatorId.ToString());
        if (operatorAccountId != accountId)
        {
            throw new ForbiddenAccessException();
        }

        var now = DateTimeOffset.UtcNow;

        var existing = await Context.Devices
            .FirstOrDefaultAsync(d => d.AccountId == accountId
                && d.OperatorId == deviceDto.OperatorId
                && d.Identifier == deviceDto.Identifier,
                cancellationToken);

        Entities.Device device;
        bool created;
        if (existing == null)
        {
            device = new Entities.Device(
                deviceDto.Name,
                deviceDto.Identifier,
                deviceDto.Serial,
                deviceDto.DeviceTypeId,
                deviceDto.Description,
                deviceDto.ProviderDisplayName,
                deviceDto.ProviderMetadataHash,
                deviceDto.ProviderStatus,
                (int)DetectedStatus.New,
                deviceDto.OperatorId,
                accountId)
            {
                FirstSeenAt = now,
                LastSeenAt = now,
                LastSyncedAt = now
            };
            await Context.Devices.AddAsync(device, cancellationToken);
            created = true;
        }
        else
        {
            existing.Name = deviceDto.Name;
            existing.Identifier = deviceDto.Identifier;
            existing.Serial = deviceDto.Serial;
            existing.DeviceTypeId = deviceDto.DeviceTypeId;
            existing.Description = deviceDto.Description;
            existing.ProviderDisplayName = deviceDto.ProviderDisplayName;
            existing.ProviderMetadataHash = deviceDto.ProviderMetadataHash;
            existing.ProviderStatus = deviceDto.ProviderStatus;
            existing.LastSeenAt = now;
            existing.LastSyncedAt = now;
            if (existing.DetectedStatus == (int)DetectedStatus.New)
            {
                existing.DetectedStatus = (int)DetectedStatus.Available;
            }
            device = existing;
            created = false;
        }

        AddAuditEvent(accountId, created ? "SynchronizedDevice.Created" : "SynchronizedDevice.Updated",
            "SynchronizedDevice", device.DeviceId.ToString(), null, null);
        await Context.SaveChangesAsync(cancellationToken);

        return new DeviceVm(
            device.DeviceId,
            device.AccountId,
            device.OperatorId,
            device.Serial,
            device.Name,
            device.Identifier,
            device.ProviderDisplayName,
            (DeviceType)device.DeviceTypeId,
            device.DeviceTypeId,
            device.Description,
            device.ProviderMetadataHash,
            device.ProviderStatus,
            (DetectedStatus)device.DetectedStatus,
            device.FirstSeenAt,
            device.LastSeenAt,
            device.LastSyncedAt,
            device.LastAssignedAt,
            device.IgnoredAt);
    }

    public async Task SetDetectedStatusAsync(Guid deviceId, DetectedStatus status, CancellationToken cancellationToken)
    {
        var device = await Context.Devices.FindAsync([deviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Entities.Device), deviceId.ToString());

        RequireAccountAccess(device.AccountId);
        device.DetectedStatus = (int)status;
        if (status == DetectedStatus.Ignored)
        {
            device.IgnoredAt = DateTimeOffset.UtcNow;
        }
        else if (device.IgnoredAt.HasValue && status != DetectedStatus.Ignored)
        {
            device.IgnoredAt = null;
        }
        AddAuditEvent(device.AccountId, "SynchronizedDevice.StatusChanged",
            "SynchronizedDevice", deviceId.ToString(), null, $"{{\"status\":\"{status}\"}}");
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await Context.Devices.FindAsync([deviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Entities.Device), deviceId.ToString());

        RequireAccountAccess(device.AccountId);
        Context.Devices.Remove(device);
        AddAuditEvent(device.AccountId, "SynchronizedDevice.Deleted",
            "SynchronizedDevice", deviceId.ToString(), null, null);
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteDevicesByOperatorAsync(Guid operatorId, CancellationToken cancellationToken)
    {
        var devices = await Context.Devices
            .Where(d => d.OperatorId == operatorId
                && (CanAccessAllAccounts || d.AccountId == Principal.AccountId))
            .ToListAsync(cancellationToken);

        if (devices.Count == 0) return 0;

        foreach (var d in devices)
        {
            Context.Devices.Remove(d);
            AddAuditEvent(d.AccountId, "SynchronizedDevice.Wiped",
                "SynchronizedDevice", d.DeviceId.ToString(), null, null);
        }
        await Context.SaveChangesAsync(cancellationToken);
        return devices.Count;
    }
}
