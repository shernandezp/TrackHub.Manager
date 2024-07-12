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
        {
            DeviceId = deviceOperatorDto.DeviceId,
            OperatorId = deviceOperatorDto.OperatorId
        };

        await context.DeviceOperators.AddAsync(deviceOperator, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceOperatorVm(
            deviceOperator.DeviceId,
            deviceOperator.OperatorId);
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
        var deviceOperator = await context.DeviceOperators.FindAsync([deviceId, operatorId], cancellationToken)
            ?? throw new NotFoundException(nameof(DeviceOperator), $"{deviceId},{operatorId}");

        context.DeviceOperators.Remove(deviceOperator);
        await context.SaveChangesAsync(cancellationToken);
    }
}
