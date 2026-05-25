using Common.Application.Exceptions;
using Common.Application.Interfaces;
using Common.Domain.Enums;
using Common.Domain.Helpers;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class DeviceReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IDeviceReader
{
    private static readonly System.Linq.Expressions.Expression<Func<Entities.Device, DeviceVm>> Projection = d => new DeviceVm(
        d.DeviceId,
        d.AccountId,
        d.OperatorId,
        d.Serial,
        d.Name,
        d.Identifier,
        d.ProviderDisplayName,
        (DeviceType)d.DeviceTypeId,
        d.DeviceTypeId,
        d.Description,
        d.ProviderMetadataHash,
        d.ProviderStatus,
        (DetectedStatus)d.DetectedStatus,
        d.FirstSeenAt,
        d.LastSeenAt,
        d.LastSyncedAt,
        d.LastAssignedAt,
        d.IgnoredAt);

    public async Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken)
    {
        var device = await Context.Devices
            .Where(d => d.DeviceId == id)
            .Select(Projection)
            .Cast<DeviceVm?>()
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(Entities.Device), id.ToString());

        RequireAccountAccess(device.AccountId);
        return device;
    }

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        return await Context.Devices
            .Where(d => d.AccountId == scoped)
            .Select(Projection)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByOperatorAsync(Guid operatorId, CancellationToken cancellationToken)
    {
        return await Context.Devices
            .Where(d => d.OperatorId == operatorId
                && (CanAccessAllAccounts || d.AccountId == Principal.AccountId))
            .Select(Projection)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeviceVm>> GetUnassignedDevicesAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        return await Context.Devices
            .Where(d => d.AccountId == scoped
                && d.IgnoredAt == null
                && !d.Assignments.Any(a => a.Status == (int)AssignmentStatus.Active))
            .Select(Projection)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<DeviceVm>> SearchDevicesAsync(Guid accountId, Filters filters, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        var query = Context.Devices.Where(d => d.AccountId == scoped);
        query = filters.Apply(query);
        return await query.Select(Projection).ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<(string Serial, Guid OperatorId)>> FindDuplicateSerialsAsync(
        Guid accountId, Guid excludeOperatorId, IReadOnlyCollection<string> serials, CancellationToken cancellationToken)
    {
        if (serials is null || serials.Count == 0)
            return [];

        var scoped = RequireAccountAccess(accountId);
        var serialSet = serials.Where(s => !string.IsNullOrWhiteSpace(s)).ToHashSet(StringComparer.Ordinal);
        if (serialSet.Count == 0)
            return [];

        var rows = await Context.Devices
            .Where(d => d.AccountId == scoped
                && d.OperatorId != excludeOperatorId
                && serialSet.Contains(d.Serial))
            .Select(d => new { d.Serial, d.OperatorId })
            .ToListAsync(cancellationToken);

        return rows.Select(r => (r.Serial, r.OperatorId)).ToList();
    }
}
