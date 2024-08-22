using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface ITransporterGroupWriter
{
    Task<TransporterGroupVm> CreateTransporterGroupAsync(TransporterGroupDto transporterGroupDto, CancellationToken cancellationToken);
    Task DeleteTransporterGroupAsync(Guid transporterId, long groupId, CancellationToken cancellationToken);
}
