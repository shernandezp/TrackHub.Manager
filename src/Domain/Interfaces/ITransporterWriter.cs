using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface ITransporterWriter
{
    Task<TransporterVm> CreateTransporterAsync(TransporterDto transporterDto, CancellationToken cancellationToken);
    Task DeleteTransporterAsync(Guid transporterId, CancellationToken cancellationToken);
    Task UpdateTransporterAsync(UpdateTransporterDto transporterDto, CancellationToken cancellationToken);
}
