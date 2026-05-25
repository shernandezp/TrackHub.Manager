using Common.Domain.Helpers;
using TrackHub.Manager.Infrastructure.Interfaces;
using TransporterType = Common.Domain.Enums.TransporterType;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class DeviceTransporterReader(IApplicationDbContext context) : IDeviceTransporterReader
{
    public async Task<IReadOnlyCollection<DeviceTransporterVm>> GetDeviceTransporterByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(t => t.Assignments)
            .Where(a => a.Status == (int)AssignmentStatus.Active && a.Device.OperatorId == operatorId)
            .Select(a => new DeviceTransporterVm(
                a.TransporterId,
                a.Device.Identifier,
                a.Device.Serial,
                a.Transporter.Name,
                (TransporterType)a.Transporter.TransporterTypeId,
                a.Transporter.TransporterTypeId))
            .Distinct()
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<DeviceTransporterVm>> GetDeviceTransportersAsync(Filters filters, CancellationToken cancellationToken)
    {
        var query = context.TransporterDeviceAssignments
            .Where(a => a.Status == (int)AssignmentStatus.Active)
            .AsQueryable();
        query = filters.Apply(query);

        return await query
            .Select(a => new DeviceTransporterVm(
                a.TransporterId,
                a.Device.Identifier,
                a.Device.Serial,
                a.Transporter.Name,
                (TransporterType)a.Transporter.TransporterTypeId,
                a.Transporter.TransporterTypeId))
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<DeviceTransporterVm> GetDeviceTransporterAsync(Guid transporterId, CancellationToken cancellationToken)
        => await context.TransporterDeviceAssignments
            .Where(a => a.TransporterId == transporterId && a.Status == (int)AssignmentStatus.Active)
            .OrderByDescending(a => a.IsPrimary)
            .ThenBy(a => a.Priority)
            .Select(a => new DeviceTransporterVm(
                a.TransporterId,
                a.Device.Identifier,
                a.Device.Serial,
                a.Transporter.Name,
                (TransporterType)a.Transporter.TransporterTypeId,
                a.Transporter.TransporterTypeId))
            .FirstAsync(cancellationToken);
}
