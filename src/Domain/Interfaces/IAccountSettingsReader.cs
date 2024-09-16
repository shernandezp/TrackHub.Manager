namespace TrackHub.Manager.Domain.Interfaces;

public interface IAccountSettingsReader
{
    Task<AccountSettingsVm> GetAccountSettingsAsync(Guid id, CancellationToken cancellationToken);
}
