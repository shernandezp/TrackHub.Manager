using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IAccountWriter
{
    Task<AccountVm> CreateAccountAsync(AccountDto accountDto, CancellationToken cancellationToken);
    Task DeleteAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task UpdateAccountAsync(UpdateAccountDto accountDto, CancellationToken cancellationToken);
}
