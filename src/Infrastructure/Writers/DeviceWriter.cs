namespace TrackHub.Manager.Infrastructure.Writers;

// This class represents a writer for Device entities
public sealed class DeviceWriter(IApplicationDbContext context) : IDeviceWriter
{
    // Creates a new Device asynchronously
    // Parameters:
    //   deviceDto: The DTO object containing the device data
    //   cancellationToken: A token to cancel the operation if needed
    // Returns:
    //   A Task representing the asynchronous operation. The task result contains the created DeviceVm
    public async Task<DeviceVm> CreateDeviceAsync(DeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = new Device
        (
            deviceDto.Name,
            deviceDto.Identifier,
            deviceDto.Serial,
            deviceDto.DeviceTypeId,
            deviceDto.Description,
            deviceDto.TransporterId,
            deviceDto.OperatorId
        );

        await context.Devices.AddAsync(device, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceVm(
            device.DeviceId,
            device.Name,
            device.Identifier,
            device.Serial,
            device.DeviceTypeId,
            device.Description,
            device.TransporterId,
            device.OperatorId);
    }


    // Updates a Device asynchronously
    // Parameters:
    //   deviceDto: The DTO object containing the updated device data
    //   cancellationToken: A token to cancel the operation if needed
    public async Task UpdateDeviceAsync(UpdateDeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync(deviceDto.DeviceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{deviceDto.DeviceId}");

        context.Devices.Attach(device);

        device.Name = deviceDto.Name;
        device.Identifier = deviceDto.Identifier;
        device.Serial = deviceDto.Serial;
        device.DeviceTypeId = deviceDto.DeviceTypeId;
        device.Description = deviceDto.Description;
        device.TransporterId = deviceDto.TransporterId;
        device.OperatorId = deviceDto.OperatorId;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes a Device asynchronously
    // Parameters:
    //   deviceId: The ID of the device associated with the Device
    //   cancellationToken: A token to cancel the operation if needed
    // Throws:
    //   NotFoundException: If the Device with the specified IDs is not found
    public async Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync([deviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceId}");

        context.Devices.Attach(device);

        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}
