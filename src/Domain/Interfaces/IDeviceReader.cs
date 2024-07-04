namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceReader
{
    Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByGroupAsync(long groupId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByGroupAsync(long groupId, Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken);
}
