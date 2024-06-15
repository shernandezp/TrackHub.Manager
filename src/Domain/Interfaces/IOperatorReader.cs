
namespace TrackHub.Manager.Domain.Interfaces;
public interface IOperatorReader
{
    Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByAccountAsync(Guid accountId, CancellationToken cancellationToken);
}
