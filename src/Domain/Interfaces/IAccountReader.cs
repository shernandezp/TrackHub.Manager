namespace TrackHub.Manager.Domain.Interfaces;
public interface IAccountReader
{
    Task<AccountVm> GetAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AccountVm>> GetAccountsAsync(CancellationToken cancellationToken);
    Task<AccountVm> GetAccountByUserIdAsync(Guid userId, CancellationToken cancellationToken);
}
