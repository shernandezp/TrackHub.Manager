using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;
using Common.Domain.Helpers;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IOperatorSyncRunReader
{
    Task<IReadOnlyCollection<OperatorSyncRunVm>> GetAsync(Filters filters, int take, CancellationToken cancellationToken);
    Task<OperatorSyncRunVm?> GetLatestAsync(Guid operatorId, CancellationToken cancellationToken);
}
