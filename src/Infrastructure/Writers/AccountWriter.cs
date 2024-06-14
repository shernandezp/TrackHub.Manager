using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class AccountWriter(IApplicationDbContext context) : IAccountWriter
{
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

    public async Task UpdateAccountAsync(UpdateAccountDto accountDto, CancellationToken cancellationToken)
    {
        var account = await context.Accounts.FindAsync([accountDto.AccountId], cancellationToken)
            ?? throw new NotFoundException(nameof(Account), $"{accountDto.AccountId}");

        account.Name = accountDto.Name;
        account.Description = accountDto.Description;
        account.Type = (short)accountDto.Type;
        account.Active = accountDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var account = await context.Accounts.FindAsync([accountId], cancellationToken)
            ?? throw new NotFoundException(nameof(Account), $"{accountId}");

        context.Accounts.Remove(account);
        await context.SaveChangesAsync(cancellationToken);
    }
}
