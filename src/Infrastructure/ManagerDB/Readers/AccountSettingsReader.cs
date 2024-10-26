using TrackHub.Manager.Domain;

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

    /// <summary>
    /// Retrieves a collection of account settings
    /// </summary>
    /// <param name="filters">Filters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Returns a collection of AccountSettingsVm objects</returns>
    public async Task<IReadOnlyCollection<AccountSettingsVm>> GetAccountSettingsAsync(Filters filters, CancellationToken cancellationToken)
    {
        var query = context.AccountSettings.AsQueryable();
        query = filters.Apply(query);

        return await query
            .Select(a => new AccountSettingsVm(
                a.AccountId,
                a.Maps,
                a.MapsKey,
                a.OnlineTimeLapse,
                a.StoreLastPosition,
                a.StoringTimeLapse,
                a.RefreshMap,
                a.RefreshMapTimer))
            .ToListAsync(cancellationToken);
    }
}
