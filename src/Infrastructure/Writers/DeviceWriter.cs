using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;
// This class represents a writer for the Device entity
public sealed class DeviceWriter(IApplicationDbContext context) : IDeviceWriter
{
    // Creates a new device asynchronously
    // Parameters:
    // - deviceDto: The device data transfer object
    // - cancellationToken: The cancellation token
    // Returns:
    // - The created device view model
    public async Task<DeviceVm> CreateDeviceAsync(DeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = new Device(
            deviceDto.Identifier,
            deviceDto.Serial,
            deviceDto.Name,
            (short)deviceDto.DeviceTypeId,
            deviceDto.Description);

        await context.Devices.AddAsync(device, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceVm(
            device.DeviceId,
            device.Identifier,
            device.Serial,
            device.Name,
            (DeviceType)device.DeviceTypeId,
            device.Description);
    }

    // Updates an existing device asynchronously
    // Parameters:
    // - deviceDto: The updated device data transfer object
    // - cancellationToken: The cancellation token
    public async Task UpdateDeviceAsync(UpdateDeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync(deviceDto.DeviceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceDto.DeviceId}");

        device.Name = deviceDto.Name;
        device.Identifier = deviceDto.Identifier;
        device.Serial = deviceDto.Serial;
        device.DeviceTypeId = (short)deviceDto.DeviceTypeId;
        device.Description = deviceDto.Description;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes a device asynchronously
    // Parameters:
    // - deviceId: The ID of the device to delete
    // - cancellationToken: The cancellation token
    public async Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync(deviceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceId}");

        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}
