namespace TrackHub.Manager.Domain.Interfaces;

public interface IOperatorReader
{
    Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUserAsync(Guid userId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByGroupAsync(long groupId, CancellationToken cancellationToken);
}
