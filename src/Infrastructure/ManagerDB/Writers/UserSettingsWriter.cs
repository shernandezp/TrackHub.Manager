namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// UserSettingsWriter class for handling user settings-related operations
public sealed class UserSettingsWriter(IApplicationDbContext context) : IUserSettingsWriter
{
    /// <summary>
    /// Creates a new user setting asynchronously
    /// </summary>
    /// <param name="userSettingsDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created user settings view model</returns>
    public async Task<UserSettingsVm> CreateUserSettingsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userSettings = new UserSettings(userId);

        await context.UserSettings.AddAsync(userSettings, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new UserSettingsVm(
            userSettings.Language,
            userSettings.Style,
            userSettings.UserId);
    }

    /// <summary>
    /// Updates an existing user setting asynchronously
    /// </summary>
    /// <param name="userSettingsDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateUserSettingsAsync(UserSettingsDto userSettingsDto, CancellationToken cancellationToken)
    {
        var userSettings = await context.UserSettings.FindAsync([userSettingsDto.UserId], cancellationToken)
            ?? throw new NotFoundException(nameof(UserSettings), $"{userSettingsDto.UserId}");

        context.UserSettings.Attach(userSettings);

        userSettings.Language = userSettingsDto.Language;
        userSettings.Style = userSettingsDto.Style;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a user setting asynchronously
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task DeleteUserSettingsAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userSettings = await context.UserSettings.FindAsync([userId], cancellationToken)
            ?? throw new NotFoundException(nameof(UserSettings), $"{userId}");

        context.UserSettings.Attach(userSettings);

        context.UserSettings.Remove(userSettings);
        await context.SaveChangesAsync(cancellationToken);
    }
}
