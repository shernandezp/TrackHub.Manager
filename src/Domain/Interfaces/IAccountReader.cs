namespace TrackHub.Manager.Domain.Interfaces;
public interface IAccountReader
{
    Task<AccountVm> GetAccountAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AccountVm>> GetAccountsAsync(CancellationToken cancellationToken);
}
