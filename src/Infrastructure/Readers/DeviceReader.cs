using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class DeviceReader(IApplicationDbContext context) : IDeviceReader
{
    // Retrieves a device by its ID
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
                d.Name,
                d.Identifier,
                d.Serial,
                (DeviceType)d.DeviceTypeId,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .FirstAsync(cancellationToken);

    // Retrieves a device by its serial number and operator ID
    // Parameters:
    // - serial: The serial number of the device
    // - operatorId: The ID of the operator
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<DeviceVm>: A task that represents the asynchronous operation. The task result contains the DeviceVm object.
    public async Task<DeviceVm> GetDeviceAsync(string serial, Guid operatorId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.Serial.ToLower() == serial.ToLower() && d.OperatorId.Equals(operatorId))
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Name,
                d.Identifier,
                d.Serial,
                (DeviceType)d.DeviceTypeId,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .FirstOrDefaultAsync(cancellationToken);

    // Retrieves a collection of devices by user ID and operator ID
    // Parameters:
    // - userId: The ID of the user
    // - operatorId: The ID of the operator
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(d => d.Devices)
            .Where(d => d.OperatorId == operatorId)
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Name,
                d.Identifier,
                d.Serial,
                (DeviceType)d.DeviceTypeId,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .ToListAsync(cancellationToken);

    // Retrieves a collection of devices by account ID
    // Parameters:
    // - accountId: The ID of the account
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId == accountId)
            .SelectMany(a => a.Operators)
            .SelectMany(d => d.Devices)
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Name,
                d.Identifier,
                d.Serial,
                (DeviceType)d.DeviceTypeId,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .ToListAsync(cancellationToken);

    // Retrieves a collection of devices by operator ID
    // Parameters:
    // - operatorId: The ID of the operator
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.
    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByOperatorAsync(Guid operatorId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.OperatorId.Equals(operatorId))
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Name,
                d.Identifier,
                d.Serial,
                (DeviceType)d.DeviceTypeId,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .ToListAsync(cancellationToken);

    // Validates whether a device exists by its serial transporter ID and device ID
    public async Task<bool> ExistDeviceAsync(Guid transporterId, Guid deviceId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.TransporterId.Equals(transporterId) && !d.DeviceId.Equals(deviceId))
            .AnyAsync(cancellationToken);
}
