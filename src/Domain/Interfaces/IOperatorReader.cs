using Common.Domain.Helpers;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IOperatorReader
{
    Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OperatorVm>> GetOperatorsAsync(Filters filters, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUserAsync(Guid userId, CancellationToken cancellationToken);
}
