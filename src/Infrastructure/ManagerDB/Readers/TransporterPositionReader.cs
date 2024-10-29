using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class TransporterPositionReader(IApplicationDbContext context) : ITransporterPositionReader
{
    /// <summary>
    /// Retrieve a collection of transporter positions by user ID and operator ID asynchronously. 
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve transporter positions for</param>
    /// <param name="operatorId">The ID of the operator to retrieve transporter positions for</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed</param>
    /// <returns>A collection of TransporterPositionVm instances representing the retrieved transporter positions</returns>
    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(t => t.Devices)
            .Where(d => d.OperatorId == operatorId)
            .Select(d => d.Transporter.Position)
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
                new(tp.DateTime, tp.Offset),
                tp.Speed,
                tp.Course,
                tp.EventId,
                tp.Address,
                tp.City,
                tp.State,
                tp.Country,
                tp.Attributes))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieve a collection of transporter positions by operator ID asynchronously.
    /// </summary>
    /// <param name="operatorId">The ID of the operator to retrieve transporter positions for</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed</param>
    /// <returns>A collection of TransporterPositionVm instances representing the retrieved transporter positions</returns>
    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid operatorId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.OperatorId == operatorId)
            .Select(d => d.Transporter.Position)
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
                new(tp.DateTime, tp.Offset),
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
