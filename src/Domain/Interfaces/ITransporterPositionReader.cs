namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransporterPositionReader
{
    Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid operatorId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken);
}
