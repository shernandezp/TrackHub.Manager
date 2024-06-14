using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class AccountReader(IApplicationDbContext context) : IAccountReader
{
    public async Task<AccountVm> GetAccountAsync(Guid id, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId.Equals(id))
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Active))
            .FirstAsync(cancellationToken);

    public async Task<IReadOnlyCollection<AccountVm>> GetAccountsAsync(CancellationToken cancellationToken)
        => await context.Accounts
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Active))
            .ToListAsync(cancellationToken);
}
