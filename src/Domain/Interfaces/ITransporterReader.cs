namespace TrackHub.Manager.Domain.Interfaces;
public interface ITransporterReader
{
    Task<TransporterVm> GetTransporterAsync(Guid id, CancellationToken cancellationToken);
}
