using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// This class represents a writer for Device entities
public sealed class DeviceWriter(IApplicationDbContext context) : IDeviceWriter
{
    /// <summary>
    /// Creates a new Device asynchronously
    /// </summary>
    /// <param name="deviceDto">The DTO object containing the device data</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the created DeviceVm</returns>
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
            (DeviceType)device.DeviceTypeId,
            device.DeviceTypeId,
            device.Description,
            device.TransporterId,
            device.OperatorId);
    }

    /// <summary>
    /// Updates a Device asynchronously
    /// </summary>
    /// <param name="deviceDto">The DTO object containing the updated device data</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">If the Device with the specified IDs is not found</exception>
    public async Task UpdateDeviceAsync(UpdateDeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync(deviceDto.DeviceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{deviceDto.DeviceId}");

        context.Devices.Attach(device);

        device.Name = deviceDto.Name;
        device.Identifier = deviceDto.Identifier;
        device.DeviceTypeId = deviceDto.DeviceTypeId;
        device.Description = deviceDto.Description;
        device.TransporterId = deviceDto.TransporterId;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a Device asynchronously
    /// </summary>
    /// <param name="deviceId">The ID of the device associated with the Device</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">If the Device with the specified IDs is not found</exception>
    public async Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync([deviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceId}");

        context.Devices.Attach(device);

        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}
