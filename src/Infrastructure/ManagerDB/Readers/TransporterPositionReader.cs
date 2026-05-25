using TrackHub.Manager.Infrastructure.Interfaces;
using TransporterType = Common.Domain.Enums.TransporterType;

namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class TransporterPositionReader(IApplicationDbContext context) : ITransporterPositionReader
{
    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .Where(t => t.Assignments.Any(a => a.Status == (int)AssignmentStatus.Active && a.Device.OperatorId == operatorId))
            .Select(t => t.Position)
            .Where(tp => tp != null)
            .Select(tp => new TransporterPositionVm(
                tp!.TransporterPositionId,
                tp.TransporterId,
                tp.Transporter.Name,
                (TransporterType)tp.Transporter.TransporterTypeId,
                tp.GeometryId,
                tp.Latitude,
                tp.Longitude,
                tp.Altitude,
                new(DateTime.SpecifyKind(tp.DateTime, DateTimeKind.Utc), DateTimeKind.Utc == DateTime.SpecifyKind(tp.DateTime, DateTimeKind.Utc).Kind ? TimeSpan.Zero : tp.Offset),
                tp.Speed,
                tp.Course,
                tp.EventId,
                tp.Address,
                tp.City,
                tp.State,
                tp.Country,
                tp.Attributes))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid operatorId, CancellationToken cancellationToken)
        => await context.TransporterDeviceAssignments
            .Where(a => a.Status == (int)AssignmentStatus.Active && a.Device.OperatorId == operatorId)
            .Select(a => a.Transporter.Position)
            .Where(tp => tp != null)
            .Distinct()
            .Select(tp => new TransporterPositionVm(
                tp!.TransporterPositionId,
                tp.TransporterId,
                tp.Transporter.Name,
                (TransporterType)tp.Transporter.TransporterTypeId,
                tp.GeometryId,
                tp.Latitude,
                tp.Longitude,
                tp.Altitude,
                new(DateTime.SpecifyKind(tp.DateTime, DateTimeKind.Utc), tp.Offset),
                tp.Speed,
                tp.Course,
                tp.EventId,
                tp.Address,
                tp.City,
                tp.State,
                tp.Country,
                tp.Attributes))
            .ToListAsync(cancellationToken);
}
