using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class DeviceReader(IApplicationDbContext context) : IDeviceReader
{
    public async Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.DeviceId.Equals(id))
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Name,
                (DeviceType)d.DeviceTypeId,
                d.Description))
            .FirstAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByGroupAsync(long groupId, CancellationToken cancellationToken)
        => await context.Groups
            .Where(g => g.GroupId == groupId)
            .SelectMany(g => g.Devices)
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Name,
                (DeviceType)d.DeviceTypeId,
                d.Description))
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByGroupAsync(long groupId, Guid operatorId, CancellationToken cancellationToken)
        => await context.Groups
            .Where(g => g.GroupId == groupId)
            .SelectMany(g => g.Devices)
            .Where(d => d.Operators.Any(o => o.OperatorId == operatorId))
            .Distinct()
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Name,
                (DeviceType)d.DeviceTypeId,
                d.Description))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .SelectMany(g => g.Devices)
            .Distinct()
            .Select(d => new DeviceVm(d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Name,
                (DeviceType)d.DeviceTypeId,
                d.Description))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .SelectMany(g => g.Devices)
            .Where(d => d.Operators.Any(o => o.OperatorId == operatorId))
            .Distinct()
            .Select(d => new DeviceVm(d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Name,
                (DeviceType)d.DeviceTypeId,
                d.Description))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId == accountId)
            .SelectMany(a => a.Groups)
            .SelectMany(g => g.Devices)
            .Distinct()
            .Select(d => new DeviceVm(d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Name,
                (DeviceType)d.DeviceTypeId,
                d.Description))
            .ToListAsync(cancellationToken);


}
