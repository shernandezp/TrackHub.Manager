namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceOperatorReader
{
    Task<DeviceOperatorVm> GetDeviceOperatorAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceOperatorVm>> GetDeviceOperatorsByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken);
}
