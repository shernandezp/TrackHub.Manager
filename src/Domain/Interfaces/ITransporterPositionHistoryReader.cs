using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;
using Common.Domain.Helpers;

namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransporterPositionHistoryReader
{
    Task<IReadOnlyCollection<TransporterPositionHistoryVm>> GetAsync(Filters filters, int take, CancellationToken cancellationToken);
}
