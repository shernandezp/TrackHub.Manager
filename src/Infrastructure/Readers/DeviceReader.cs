using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;

// DeviceReader class for reading device information
public sealed class DeviceReader(IApplicationDbContext context) : IDeviceReader
{
    // GetDeviceAsync method retrieves a device by its ID
    // Parameters:
    // - id: The ID of the device
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<DeviceVm>: A task that represents the asynchronous operation. The task result contains the DeviceVm object.
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

    // GetDevicesByGroupAsync method retrieves devices by group ID
    // Parameters:
    // - groupId: The ID of the group
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
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

    // GetDevicesByGroupAsync method retrieves devices by group ID and operator ID
    // Parameters:
    // - groupId: The ID of the group
    // - operatorId: The ID of the operator
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
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

    // GetDevicesByUserAsync method retrieves devices by user ID
    // Parameters:
    // - userId: The ID of the user
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
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

    // GetDevicesByUserAsync method retrieves devices by user ID and operator ID
    // Parameters:
    // - userId: The ID of the user
    // - operatorId: The ID of the operator
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
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

    // GetDevicesByAccountAsync method retrieves devices by account ID
    // Parameters:
    // - accountId: The ID of the account
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
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
