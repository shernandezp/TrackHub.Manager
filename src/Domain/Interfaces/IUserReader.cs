namespace TrackHub.Manager.Domain.Interfaces;
public interface IUserReader
{
    Task<UserVm> GetUserAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<UserVm>> GetUsersByAccountAsync(Guid accountId, CancellationToken cancellationToken);
}
