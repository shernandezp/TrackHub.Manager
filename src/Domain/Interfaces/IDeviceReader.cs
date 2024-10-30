using Common.Domain.Helpers;
namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceReader
{
    Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken);
    Task<DeviceVm> GetDeviceAsync(string serial, Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceTransporterVm>> GetDeviceTransporterByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceTransporterVm>> GetDeviceTransportersAsync(Filters filters, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByOperatorAsync(Guid operatorId, CancellationToken cancellationToken);
    Task<bool> ExistDeviceAsync(Guid transporterId, Guid deviceId, CancellationToken cancellationToken);
}
