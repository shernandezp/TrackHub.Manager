namespace TrackHub.Manager.Domain.Interfaces;
public interface IGroupReader
{
    Task<GroupVm> GetGroupAsync(long id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GroupVm>> GetGroupsByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<GroupVm>> GetGroupsByUserAsync(Guid userId, CancellationToken cancellationToken);
}
