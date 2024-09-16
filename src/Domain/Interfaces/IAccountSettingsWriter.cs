using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IAccountSettingsWriter
{
    Task<AccountSettingsVm> CreateAccountSettingsAsync(Guid accountId, CancellationToken cancellationToken);
    Task UpdateAccountSettingsAsync(AccountSettingsDto accountSettingsDto, CancellationToken cancellationToken);
}
