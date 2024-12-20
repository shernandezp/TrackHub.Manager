﻿namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// AccountSettingsWriter class is responsible for writing account settings-related data to the database
public sealed class AccountSettingsWriter(IApplicationDbContext context) : IAccountSettingsWriter
{
    /// <summary>
    /// Creates a new account setting asynchronously
    /// </summary>
    /// <param name="accountSettingsDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created account settings view model</returns>
    public async Task<AccountSettingsVm> CreateAccountSettingsAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var accountSettings = new AccountSettings(accountId);

        await context.AccountSettings.AddAsync(accountSettings, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AccountSettingsVm(
            accountSettings.AccountId,
            accountSettings.Maps,
            accountSettings.MapsKey,
            accountSettings.OnlineInterval,
            accountSettings.StoreLastPosition,
            accountSettings.StoringInterval,
            accountSettings.RefreshMap,
            accountSettings.RefreshMapInterval);
    }

    /// <summary>
    /// Updates an existing account setting asynchronously
    /// </summary>
    /// <param name="accountSettingsDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateAccountSettingsAsync(AccountSettingsDto accountSettingsDto, CancellationToken cancellationToken)
    {
        var accountSettings = await context.AccountSettings.FindAsync([accountSettingsDto.AccountId, cancellationToken], cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(AccountSettings), $"{accountSettingsDto.AccountId}");

        context.AccountSettings.Attach(accountSettings);

        accountSettings.Maps = accountSettingsDto.Maps;
        accountSettings.MapsKey = accountSettingsDto.MapsKey;
        accountSettings.OnlineInterval = accountSettingsDto.OnlineInterval;
        accountSettings.StoreLastPosition = accountSettingsDto.StoreLastPosition;
        accountSettings.StoringInterval = accountSettingsDto.StoringInterval;
        accountSettings.RefreshMap = accountSettingsDto.RefreshMap;
        accountSettings.RefreshMapInterval = accountSettingsDto.RefreshMapInterval;

        await context.SaveChangesAsync(cancellationToken);
    }
}
