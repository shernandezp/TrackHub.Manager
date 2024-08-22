namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class DeviceReader(IApplicationDbContext context) : IDeviceReader
{
    // Retrieves a device by its ID
    public async Task<DeviceVm> GetDeviceAsync(Guid id, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.DeviceId.Equals(id))
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Name,
                d.Identifier,
                d.Serial,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .FirstAsync(cancellationToken);

    // Retrieves a collection of devices by user ID and operator ID
    public async Task<IReadOnlyCollection<DeviceVm>> GetDevicesByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(d => d.Devices)
            .Where(d => d.OperatorId == operatorId)
            .Select(d => new DeviceVm(
                d.DeviceId,
                d.Name,
                d.Identifier,
                d.Serial,
                d.DeviceTypeId,
                d.Description,
                d.TransporterId,
                d.OperatorId))
            .ToListAsync(cancellationToken);

}
