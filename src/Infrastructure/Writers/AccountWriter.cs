using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;

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
            (short)accountDto.Type,
            accountDto.Active);

        await context.Accounts.AddAsync(account, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AccountVm(
            account.AccountId,
            account.Name,
            account.Description,
            (AccountType)account.Type,
            account.Active);
    }

    // Updates an existing account asynchronously
    // Parameters:
    // - accountDto: The updated account data transfer object
    // - cancellationToken: The cancellation token
    public async Task UpdateAccountAsync(UpdateAccountDto accountDto, CancellationToken cancellationToken)
    {
        var account = await context.Accounts.FindAsync(accountDto.AccountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Account), $"{accountDto.AccountId}");

        account.Name = accountDto.Name;
        account.Description = accountDto.Description;
        account.Type = (short)accountDto.Type;
        account.Active = accountDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes an account asynchronously
    // Parameters:
    // - accountId: The ID of the account to delete
    // - cancellationToken: The cancellation token
    public async Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await context.Accounts.FindAsync(accountId, cancellationToken)
            ?? throw new NotFoundException(nameof(Account), $"{accountId}");

        context.Accounts.Remove(account);
        await context.SaveChangesAsync(cancellationToken);
    }
}
