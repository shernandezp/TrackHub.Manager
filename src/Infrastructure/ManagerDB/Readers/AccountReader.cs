﻿using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// AccountReader class for reading account data
public sealed class AccountReader(IApplicationDbContext context) : IAccountReader
{
    // Retrieves an account by ID
    // Returns an AccountVm object
    public async Task<AccountVm> GetAccountAsync(Guid id, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId.Equals(id))
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Type,
                a.Active,
                a.LastModified))
            .FirstAsync(cancellationToken);

    // Retrieves all accounts
    // Returns a collection of AccountVm objects
    public async Task<IReadOnlyCollection<AccountVm>> GetAccountsAsync(CancellationToken cancellationToken)
        => await context.Accounts
            .Select(a => new AccountVm(
                a.AccountId,
                a.Name,
                a.Description,
                (AccountType)a.Type,
                a.Type,
                a.Active,
                a.LastModified))
            .ToListAsync(cancellationToken);
}
