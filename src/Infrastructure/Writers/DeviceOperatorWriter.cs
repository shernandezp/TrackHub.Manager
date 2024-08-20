namespace TrackHub.Manager.Infrastructure.Writers;

// This class represents a writer for DeviceOperator entities
public sealed class DeviceOperatorWriter(IApplicationDbContext context) : IDeviceOperatorWriter
{
    // Creates a new DeviceOperator asynchronously
    // Parameters:
    //   deviceOperatorDto: The DTO object containing the device operator data
    //   cancellationToken: A token to cancel the operation if needed
    // Returns:
    //   A Task representing the asynchronous operation. The task result contains the created DeviceOperatorVm
    public async Task<DeviceOperatorVm> CreateDeviceOperatorAsync(DeviceOperatorDto deviceOperatorDto, CancellationToken cancellationToken)
    {
        var deviceOperator = new DeviceOperator
        (
            deviceOperatorDto.Identifier,
            deviceOperatorDto.Serial,
            deviceOperatorDto.DeviceId,
            deviceOperatorDto.OperatorId
        );

        await context.DevicesOperator.AddAsync(deviceOperator, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceOperatorVm(
            deviceOperator.DeviceOperatorId,
            "",
            deviceOperator.Identifier,
            deviceOperator.Serial,
            deviceOperator.DeviceId,
            deviceOperator.OperatorId);
    }


    // Updates a DeviceOperator asynchronously
    // Parameters:
    //   deviceOperatorDto: The DTO object containing the updated device operator data
    //   cancellationToken: A token to cancel the operation if needed
    public async Task UpdateDeviceOperatorAsync(UpdateDeviceOperatorDto deviceOperatorDto, CancellationToken cancellationToken)
    {
        var deviceOperator = await context.DevicesOperator.FindAsync(deviceOperatorDto.DeviceOperatorId, cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceOperatorDto.DeviceOperatorId}");

        context.DevicesOperator.Attach(deviceOperator);

        deviceOperator.Identifier = deviceOperatorDto.Identifier;
        deviceOperator.Serial = deviceOperatorDto.Serial;
        deviceOperator.DeviceId = deviceOperatorDto.DeviceId;
        deviceOperator.OperatorId = deviceOperatorDto.OperatorId;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes a DeviceOperator asynchronously
    // Parameters:
    //   deviceId: The ID of the device associated with the DeviceOperator
    //   operatorId: The ID of the operator associated with the DeviceOperator
    //   cancellationToken: A token to cancel the operation if needed
    // Throws:
    //   NotFoundException: If the DeviceOperator with the specified IDs is not found
    public async Task DeleteDeviceOperatorAsync(Guid deviceId, Guid operatorId, CancellationToken cancellationToken)
    {
        var deviceOperator = await context.DevicesOperator.FindAsync([deviceId, operatorId], cancellationToken)
            ?? throw new NotFoundException(nameof(DeviceOperator), $"{deviceId},{operatorId}");

        context.DevicesOperator.Attach(deviceOperator);

        context.DevicesOperator.Remove(deviceOperator);
        await context.SaveChangesAsync(cancellationToken);
    }
}
