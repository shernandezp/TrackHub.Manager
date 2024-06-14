namespace TrackHub.Manager.Domain.Interfaces;
public interface IGroupReader
{
    Task<GroupVm> GetGroupAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GroupVm>> GetGroupsByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GroupVm>> GetGroupsByUserAsync(Guid userId, CancellationToken cancellationToken);
}
