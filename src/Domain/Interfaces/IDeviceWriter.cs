using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IDeviceWriter
{
    Task<DeviceVm> CreateDeviceAsync(DeviceDto deviceDto, CancellationToken cancellationToken);
    Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken);
    Task UpdateDeviceAsync(UpdateDeviceDto deviceDto, CancellationToken cancellationToken);
}
