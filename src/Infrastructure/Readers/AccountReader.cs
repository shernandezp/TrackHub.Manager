using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;

// AccountReader class for reading account data
public sealed class AccountReader(IApplicationDbContext context) : IAccountReader
{
    // Retrieves an account by ID
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

    // Retrieves all accounts
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
