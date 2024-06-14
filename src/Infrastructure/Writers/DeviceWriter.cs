using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class DeviceWriter(IApplicationDbContext context) : IDeviceWriter
{
    public async Task<DeviceVm> CreateDeviceAsync(DeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = new Device(
            deviceDto.Identifier,
            deviceDto.Name,
            (short)deviceDto.DeviceTypeId,
            deviceDto.Description);

        await context.Devices.AddAsync(device, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceVm(
            device.DeviceId,
            device.Identifier,
            device.Name,
            (DeviceType)device.DeviceTypeId,
            device.Description);
    }

    public async Task UpdateDeviceAsync(UpdateDeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync([deviceDto.DeviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceDto.DeviceId}");

        device.Name = deviceDto.Name;
        device.Identifier = deviceDto.Identifier;
        device.DeviceTypeId = (short)deviceDto.DeviceTypeId;
        device.Description = deviceDto.Description;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync([deviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceId}");

        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}
