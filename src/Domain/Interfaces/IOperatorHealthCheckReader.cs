using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IOperatorHealthCheckReader
{
    Task<IReadOnlyCollection<OperatorHealthCheckVm>> GetByOperatorAsync(Guid operatorId, int take, CancellationToken cancellationToken);
    Task<OperatorHealthVm> GetLatestHealthAsync(Guid operatorId, CancellationToken cancellationToken);
    Task<OperatorHealthSummaryVm> GetSummaryAsync(Guid operatorId, DateTimeOffset since, CancellationToken cancellationToken);
    Task<DateTimeOffset?> GetLastCheckAtAsync(Guid operatorId, CancellationToken cancellationToken);
}
