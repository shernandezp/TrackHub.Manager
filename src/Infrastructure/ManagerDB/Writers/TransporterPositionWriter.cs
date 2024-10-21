namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class TransporterPositionWriter(IApplicationDbContext context) : ITransporterPositionWriter
{

    /// <summary>
    /// This method will create a new TransporterPosition in the database
    /// </summary>
    /// <param name="positionDto">The TransporterPosition data transfer object</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The TransporterPosition view model</returns>
    public async Task<TransporterPositionVm> CreateTransporterPositionAsync(TransporterPositionDto positionDto, CancellationToken cancellationToken)
    {
        var position = new TransporterPosition
        (
            positionDto.TransporterId,
            positionDto.GeometryId,
            positionDto.Latitude,
            positionDto.Longitude,
            positionDto.Altitude,
            positionDto.DeviceDateTime,
            positionDto.Speed,
            positionDto.Course,
            positionDto.EventId,
            positionDto.Address,
            positionDto.City,
            positionDto.State,
            positionDto.Country,
            positionDto.Attributes
        );

        await context.TransporterPositions.AddAsync(position, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TransporterPositionVm(
            position.TransporterPositionId,
            position.TransporterId,
            position.GeometryId,
            position.Latitude,
            position.Longitude,
            position.Altitude,
            position.DeviceDateTime,
            position.Speed,
            position.Course,
            position.EventId,
            position.Address,
            position.City,
            position.State,
            position.Country,
            position.Attributes);
    }

    /// <summary>
    /// Bulk insert transporter positions
    /// </summary>
    /// <param name="positionsDto"></param>
    /// <param name="cancellationToken">Task</param>
    /// <returns></returns>
    public async Task BulkTransporterPositionAsync(IEnumerable<TransporterPositionDto> positionsDto, CancellationToken cancellationToken)
    {
        var positions = positionsDto.Select(positionDto => new TransporterPosition
            (
                positionDto.TransporterId,
                positionDto.GeometryId,
                positionDto.Latitude,
                positionDto.Longitude,
                positionDto.Altitude,
                positionDto.DeviceDateTime,
                positionDto.Speed,
                positionDto.Course,
                positionDto.EventId,
                positionDto.Address,
                positionDto.City,
                positionDto.State,
                positionDto.Country,
                positionDto.Attributes
            )).ToList();

        await context.BulkInsertAsync(positions, "transporterid", cancellationToken);
    }

    /// <summary>
    /// UpdateTransporterPositionAsync method is used to update the existing TransporterPosition in the database
    /// </summary>
    /// <param name="positionDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Task</returns>
    /// <exception cref="NotFoundException">if the transporter position is not found</exception>
    public async Task UpdateTransporterPositionAsync(TransporterPositionDto positionDto, CancellationToken cancellationToken)
    {
        var position = await context.TransporterPositions.FirstOrDefaultAsync(t => t.TransporterId == positionDto.TransporterId, cancellationToken)
            ?? throw new NotFoundException(nameof(TransporterPosition), $"{positionDto.TransporterId}");

        context.TransporterPositions.Attach(position);

        position.GeometryId = positionDto.GeometryId;
        position.Latitude = positionDto.Latitude;
        position.Longitude = positionDto.Longitude;
        position.Altitude = positionDto.Altitude;
        position.DeviceDateTime = positionDto.DeviceDateTime;
        position.Speed = positionDto.Speed;
        position.Course = positionDto.Course;
        position.EventId = positionDto.EventId;
        position.Address = positionDto.Address;
        position.City = positionDto.City;
        position.State = positionDto.State;
        position.Country = positionDto.Country;
        position.Attributes = positionDto.Attributes;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// This method will delete an existing TransporterPosition in the database
    /// </summary>
    /// <param name="transporterId">The TransporterPosition identifier</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public async Task DeleteTransporterPositionAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        var position = await context.TransporterPositions.FirstOrDefaultAsync(t => t.TransporterId == transporterId, cancellationToken);

        if (position is not null)
        {
            context.TransporterPositions.Attach(position);

            context.TransporterPositions.Remove(position);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
