using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IDeviceGroupWriter
{
    Task<DeviceGroupVm> CreateDeviceGroupAsync(DeviceGroupDto deviceGroupDto, CancellationToken cancellationToken);
    Task DeleteDeviceGroupAsync(Guid deviceId, long groupId, CancellationToken cancellationToken);
}
