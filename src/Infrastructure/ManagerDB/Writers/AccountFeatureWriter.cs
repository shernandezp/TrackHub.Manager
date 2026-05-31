using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class AccountFeatureWriter(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IAccountFeatureWriter
{
    public async Task<AccountFeatureVm> SetAccountFeatureAsync(AccountFeatureDto feature, CancellationToken cancellationToken)
    {
        var accountId = RequireAccountAccess(feature.AccountId);
        var entity = await Context.AccountFeatures.FirstOrDefaultAsync(x => x.AccountId == accountId && x.FeatureKey == feature.FeatureKey, cancellationToken);
        string? oldValues = null;
        if (entity == null)
        {
            entity = new AccountFeature(accountId, feature.FeatureKey, feature.Enabled, feature.Tier, feature.Source, feature.EffectiveFrom, feature.EffectiveTo, feature.ConfigurationJson);
            await Context.AccountFeatures.AddAsync(entity, cancellationToken);
        }
        else
        {
            oldValues = AuditValues(entity);
            Context.AccountFeatures.Attach(entity);
            entity.Enabled = feature.Enabled;
            entity.Tier = feature.Tier;
            entity.Source = feature.Source;
            entity.EffectiveFrom = feature.EffectiveFrom;
            entity.EffectiveTo = feature.EffectiveTo;
            entity.ConfigurationJson = feature.ConfigurationJson;
        }

        AddAuditEvent(accountId, "SetAccountFeature", "AccountFeature", entity.AccountFeatureId.ToString(), oldValues, AuditValues(entity));
        await Context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task DisableAccountFeatureAsync(Guid accountFeatureId, CancellationToken cancellationToken)
    {
        var entity = await Context.AccountFeatures.FirstAsync(x => x.AccountFeatureId == accountFeatureId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        Context.AccountFeatures.Attach(entity);
        var oldValues = AuditValues(entity);
        entity.Enabled = false;
        AddAuditEvent(entity.AccountId, "DisableAccountFeature", "AccountFeature", entity.AccountFeatureId.ToString(), oldValues, AuditValues(entity));
        await Context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAccountFeatureConfigurationAsync(Guid accountFeatureId, string? configurationJson, CancellationToken cancellationToken)
    {
        var entity = await Context.AccountFeatures.FirstAsync(x => x.AccountFeatureId == accountFeatureId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        Context.AccountFeatures.Attach(entity);
        var oldValues = AuditValues(entity);
        entity.ConfigurationJson = configurationJson;
        AddAuditEvent(entity.AccountId, "UpdateAccountFeatureConfiguration", "AccountFeature", entity.AccountFeatureId.ToString(), oldValues, AuditValues(entity));
        await Context.SaveChangesAsync(cancellationToken);
    }

    private static AccountFeatureVm ToVm(AccountFeature x) 
        => new(x.AccountFeatureId, x.AccountId, x.FeatureKey, x.Enabled, x.Tier, x.Source, x.EffectiveFrom, x.EffectiveTo, x.ConfigurationJson, x.LastModified);

    private static string AuditValues(AccountFeature feature) 
        => $$"""{"featureKey":"{{feature.FeatureKey}}","enabled":{{feature.Enabled.ToString().ToLowerInvariant()}},"tier":"{{feature.Tier}}","source":"{{feature.Source}}","effectiveFrom":{{Quote(feature.EffectiveFrom)}},"effectiveTo":{{Quote(feature.EffectiveTo)}},"configurationJson":{{Quote(feature.ConfigurationJson)}}}""";

}
