using Common.Domain.Helpers;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceReader
{
    Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetDevicesByOperatorAsync(Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> GetUnassignedDevicesAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<DeviceVm>> SearchDevicesAsync(Guid accountId, Filters filters, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<(string Serial, Guid OperatorId)>> FindDuplicateSerialsAsync(
        Guid accountId, Guid excludeOperatorId, IReadOnlyCollection<string> serials, CancellationToken cancellationToken);
}
