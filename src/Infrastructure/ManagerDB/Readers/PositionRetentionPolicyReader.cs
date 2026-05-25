using System.Text.Json;
using Common.Application.Interfaces;
using Common.Domain.Constants;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class PositionRetentionPolicyReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IPositionRetentionPolicyReader
{
    public async Task<PositionRetentionPolicyVm> GetAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        var feature = await Context.AccountFeatures
            .Where(f => f.AccountId == scoped && f.FeatureKey == FeatureKeys.GpsPositionHistory)
            .OrderByDescending(f => f.LastModified)
            .FirstOrDefaultAsync(cancellationToken);

        if (feature is null || !feature.Enabled)
        {
            return new PositionRetentionPolicyVm(false, 0, true, "Default");
        }

        if (!string.IsNullOrWhiteSpace(feature.ConfigurationJson))
        {
            try
            {
                var doc = JsonDocument.Parse(feature.ConfigurationJson!);
                var root = doc.RootElement;
                var retention = root.TryGetProperty("retentionDays", out var rd) ? rd.GetInt32() : 30;
                var latestOnly = root.TryGetProperty("latestOnly", out var lo) && lo.GetBoolean();
                return new PositionRetentionPolicyVm(true, retention, latestOnly, feature.Source);
            }
            catch
            {
                // fall through to default
            }
        }
        return new PositionRetentionPolicyVm(true, 30, false, feature.Source);
    }
}

public sealed class PositionRetentionPolicyWriter(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IPositionRetentionPolicyWriter
{
    public async Task SetAsync(Guid accountId, PositionRetentionPolicyDto dto, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        var feature = await Context.AccountFeatures
            .FirstOrDefaultAsync(f => f.AccountId == scoped && f.FeatureKey == FeatureKeys.GpsPositionHistory, cancellationToken);
        var json = JsonSerializer.Serialize(new { retentionDays = dto.RetentionDays, latestOnly = dto.LatestOnly });
        if (feature is null)
        {
            feature = new AccountFeature(scoped, FeatureKeys.GpsPositionHistory, dto.HistoryEnabled, "standard", "AccountAdmin", DateTimeOffset.UtcNow, null, json);
            await Context.AccountFeatures.AddAsync(feature, cancellationToken);
        }
        else
        {
            feature.Enabled = dto.HistoryEnabled;
            feature.ConfigurationJson = json;
        }
        AddAuditEvent(scoped, "SetPositionRetentionPolicy", nameof(AccountFeature), feature.AccountFeatureId.ToString(), null, json);
        await Context.SaveChangesAsync(cancellationToken);
    }
}
