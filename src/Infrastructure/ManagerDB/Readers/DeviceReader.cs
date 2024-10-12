using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class DeviceReader(IApplicationDbContext context) : IDeviceReader
{

    /// <summary>
    /// Retrieves a device by its ID
    /// </summary>
    /// <param name="id">The ID of the device</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<DeviceVm>: A task that represents the asynchronous operation. The task result contains the DeviceVm object.</returns>
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

    /// <summary>
    /// Retrieves a device by its serial number and operator ID 
    /// </summary>
    /// <param name="serial">The serial number of the device</param>
    /// <param name="operatorId">The ID of the operator</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<DeviceVm>: A task that represents the asynchronous operation. The task result contains the DeviceVm object.</returns>
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

    /// <summary>
    /// Retrieves a collection of devices by user ID and operator ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="operatorId">The ID of the operator</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<IReadOnlyCollection<DeviceTransporterVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.</returns>
    public async Task<IReadOnlyCollection<DeviceTransporterVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(d => d.Devices)
            .Where(d => d.OperatorId == operatorId)
            .Select(d => new DeviceTransporterVm(
                d.DeviceId,
                d.Identifier,
                d.Serial,
                d.Transporter.Name,
                (TransporterType)d.Transporter.TransporterTypeId,
                d.Transporter.TransporterTypeId))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieves a collection of devices by account ID
    /// </summary>
    /// <param name="accountId">The ID of the account</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.</returns>
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

    /// <summary>
    /// Retrieves a collection of devices by operator ID
    /// </summary>
    /// <param name="operatorId">The ID of the operator</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<IReadOnlyCollection<DeviceVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.</returns>
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

    /// <summary>
    /// Validates whether a device exists by its serial transporter ID and device ID
    /// </summary>
    /// <param name="transporterId"></param>
    /// <param name="deviceId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>returns true if the device exists, otherwise false</returns>
    public async Task<bool> ExistDeviceAsync(Guid transporterId, Guid deviceId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.TransporterId.Equals(transporterId) && !d.DeviceId.Equals(deviceId))
            .AnyAsync(cancellationToken);
}
