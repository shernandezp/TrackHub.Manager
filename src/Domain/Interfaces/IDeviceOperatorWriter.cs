using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IDeviceOperatorWriter
{
    Task<DeviceOperatorVm> CreateDeviceOperatorAsync(DeviceOperatorDto deviceOperatorDto, CancellationToken cancellationToken);
    Task UpdateDeviceOperatorAsync(UpdateDeviceOperatorDto deviceOperatorDto, CancellationToken cancellationToken);
    Task DeleteDeviceOperatorAsync(Guid deviceId, Guid operatorId, CancellationToken cancellationToken);
}
