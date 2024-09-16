namespace TrackHub.Manager.Domain.Interfaces;

public interface IUserSettingsReader
{
    Task<UserSettingsVm> GetUserSettingsAsync(Guid id, CancellationToken cancellationToken);
}
