namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// AccountSettingsReader class for reading account settings data
public sealed class AccountSettingsReader(IApplicationDbContext context) : IAccountSettingsReader
{
    /// <summary>
    /// Retrieves an account settings by ID
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>Returns an AccountSettingsVm object</returns>
    public async Task<AccountSettingsVm> GetAccountSettingsAsync(Guid id, CancellationToken cancellationToken)
        => await context.AccountSettings
            .Where(a => a.AccountId.Equals(id))
            .Select(a => new AccountSettingsVm(
                a.AccountId,
                a.Maps,
                a.MapsKey,
                a.OnlineTimeLapse,
                a.StoreLastPosition,
                a.StoringTimeLapse,
                a.RefreshMap,
                a.RefreshMapTimer))
            .FirstAsync(cancellationToken);

}
