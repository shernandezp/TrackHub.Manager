using Common.Domain.Helpers;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IAccountSettingsReader
{
    Task<AccountSettingsVm> GetAccountSettingsAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<AccountSettingsVm>> GetAccountSettingsAsync(Filters filters, CancellationToken cancellationToken);
}
