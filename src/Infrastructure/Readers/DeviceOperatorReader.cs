namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class DeviceOperatorReader(IApplicationDbContext context) : IDeviceOperatorReader
{
    // Retrieves a device operator by its ID
    public async Task<DeviceOperatorVm> GetDeviceOperatorAsync(long id, CancellationToken cancellationToken)
        => await context.DevicesOperator
            .Where(d => d.DeviceOperatorId.Equals(id))
            .Select(d => new DeviceOperatorVm(
                d.DeviceOperatorId,
                "",
                d.Identifier,
                d.Serial,
                d.DeviceId,
                d.OperatorId))
            .FirstAsync(cancellationToken);

    // Retrieves a collection of device operators by user ID and operator ID
    public async Task<IReadOnlyCollection<DeviceOperatorVm>> GetDeviceOperatorsByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Devices)
            .SelectMany(d => d.DeviceOperator)
            .Where(d => d.OperatorId == operatorId)
            .Select(d => new DeviceOperatorVm(
                d.DeviceOperatorId,
                d.Device.Name,
                d.Identifier,
                d.Serial,
                d.DeviceId,
                d.OperatorId))
            .ToListAsync(cancellationToken);

}
