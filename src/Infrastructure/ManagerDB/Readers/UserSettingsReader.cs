namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// UserSettingsReader class for reading user settings data
public sealed class UserSettingsReader(IApplicationDbContext context) : IUserSettingsReader
{
    /// <summary>
    /// Retrieves a user settings by ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns an UserSettingsVm object</returns>
    public async Task<UserSettingsVm> GetUserSettingsAsync(Guid id, CancellationToken cancellationToken)
        => await context.UserSettings
            .Where(a => a.UserId.Equals(id))
            .Select(a => new UserSettingsVm(
                a.Language,
                a.Style,
                a.Navbar,
                a.UserId))
            .FirstAsync(cancellationToken);

}
