using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransporterPositionWriter
{
    Task BulkTransporterPositionAsync(IEnumerable<TransporterPositionDto> positionsDto, CancellationToken cancellationToken);
    Task DeleteTransporterPositionAsync(Guid transporterId, CancellationToken cancellationToken);
    Task UpdateTransporterPositionAsync(TransporterPositionDto positionDto, CancellationToken cancellationToken);
}
