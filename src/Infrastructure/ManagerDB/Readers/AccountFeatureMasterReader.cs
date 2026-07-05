using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

/// <summary>
/// Cross-account read of account features for the SuperAdministrator. Not account-scoped;
/// the AccountFeaturesMaster resource authorization gates access.
/// </summary>
public sealed class AccountFeatureMasterReader(IApplicationDbContext context) : IAccountFeatureMasterReader
{
    public async Task<IReadOnlyCollection<AccountFeatureVm>> GetAccountFeaturesAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.AccountFeatures
            .Where(x => x.AccountId == accountId)
            .OrderBy(x => x.FeatureKey)
            .Select(x => new AccountFeatureVm(x.AccountFeatureId, x.AccountId, x.FeatureKey, x.Enabled, x.Tier, x.Source, x.EffectiveFrom, x.EffectiveTo, x.ConfigurationJson, x.LastModified))
            .ToListAsync(cancellationToken);
}
