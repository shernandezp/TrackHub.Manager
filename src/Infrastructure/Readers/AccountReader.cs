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
                a.Active,
                a.LastModified))
            .FirstAsync(cancellationToken);

    // Retrieves all accounts
    public async Task<IReadOnlyCollection<AccountVm>> GetAccountsAsync(CancellationToken cancellationToken)
        => await context.Accounts
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Active,
                a.LastModified))
            .ToListAsync(cancellationToken);

    // Retrieves an account by user ID
    public async Task<AccountVm> GetAccountByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
                .Where(u => u.UserId == userId)
                .Select(u => new AccountVm(
                    u.Account.AccountId,
                    u.Account.Name,
                    u.Account.Description,
                    (AccountType)u.Account.Type,
                    u.Account.Active,
                    u.Account.LastModified))
                .FirstAsync(cancellationToken);
}
