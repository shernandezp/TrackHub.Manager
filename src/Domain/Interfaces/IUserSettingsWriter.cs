using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IUserSettingsWriter
{
    Task<UserSettingsVm> CreateUserSettingsAsync(Guid userId, CancellationToken cancellationToken);
    Task DeleteUserSettingsAsync(Guid userId, CancellationToken cancellationToken);
    Task UpdateUserSettingsAsync(UserSettingsDto userSettingsDto, CancellationToken cancellationToken);
}
