namespace TrackHub.Manager.Infrastructure.Writers;

public sealed class DeviceOperatorWriter(IApplicationDbContext context) : IDeviceOperatorWriter
{
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

    public async Task DeleteDeviceOperatorAsync(Guid deviceId, Guid operatorId, CancellationToken cancellationToken)
    {
        var deviceOperator = await context.DeviceOperators.FindAsync([deviceId, operatorId], cancellationToken)
            ?? throw new NotFoundException(nameof(DeviceOperator), $"{deviceId},{operatorId}");

        context.DeviceOperators.Remove(deviceOperator);
        await context.SaveChangesAsync(cancellationToken);
    }
}
