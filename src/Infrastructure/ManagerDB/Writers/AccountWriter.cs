using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// AccountWriter class is responsible for writing account-related data to the database
public sealed class AccountWriter(IApplicationDbContext context) : IAccountWriter
{
    // Creates a new account asynchronously
    // Parameters:
    // - accountDto: The account data transfer object
    // - cancellationToken: The cancellation token
    // Returns:
    // - The created account view model
    public async Task<AccountVm> CreateAccountAsync(AccountDto accountDto, CancellationToken cancellationToken)
    {
        var account = new Account(
            accountDto.Name,
            accountDto.Description,
            accountDto.TypeId,
            accountDto.Active);

        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AccountVm(
            account.AccountId,
            account.Name,
            account.Description,
            (AccountType)account.Type,
            account.Type,
            account.Active,
            account.LastModified);
    }

    // Updates an existing account asynchronously
    // Parameters:
    // - accountDto: The updated account data transfer object
    // - cancellationToken: The cancellation token
    public async Task UpdateAccountAsync(UpdateAccountDto accountDto, CancellationToken cancellationToken)
    {
        var account = await context.Accounts.FindAsync(accountDto.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Account), $"{accountDto.AccountId}");

        context.Accounts.Attach(account);

        account.Name = accountDto.Name;
        account.Description = accountDto.Description;
        account.Type = accountDto.TypeId;
        account.Active = accountDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }
}
