namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class TransporterPositionWriter(IApplicationDbContext context) : ITransporterPositionWriter
{

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
                positionDto.Attributes == null ? null :
                    new AttributesVm
                    (
                        positionDto.Attributes?.Ignition,
                        positionDto.Attributes?.Satellites,
                        positionDto.Attributes?.Mileage,
                        positionDto.Attributes?.HobbsMeter,
                        positionDto.Attributes?.Temperature
                    )
            )).ToList();

        foreach (var position in positions)
        {
            await context.TransporterPositions.AddOrUpdateAsync(position, p => p.TransporterId, ["TransporterPositionId"], cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
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
        position.Attributes = positionDto.Attributes == null ? null : new AttributesVm(
            positionDto.Attributes?.Ignition,
            positionDto.Attributes?.Satellites,
            positionDto.Attributes?.Mileage,
            positionDto.Attributes?.HobbsMeter,
            positionDto.Attributes?.Temperature
        );

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
