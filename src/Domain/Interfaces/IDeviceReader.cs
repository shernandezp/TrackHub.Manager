namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceReader
{
    Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken);
    Task<DeviceVm> GetDeviceAsync(string serial, Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByOperatorAsync(Guid operatorId, CancellationToken cancellationToken);
    Task<bool> ExistDeviceAsync(Guid transporterId, Guid deviceId, CancellationToken cancellationToken);
}
