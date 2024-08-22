namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceReader
{
    Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken);
}
