using Common.Application.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using TrackHub.Manager.Domain.Enums;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class GpsIntegrationDashboardReader(IApplicationDbContext context, ICurrentPrincipal principal, IMemoryCache cache)
    : AccountScopedDataAccess(context, principal), IGpsIntegrationDashboardReader
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromSeconds(30);

    public Task<GpsIntegrationDashboardVm> GetAsync(Guid accountId, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        var key = $"gps-dashboard:{scoped:N}";
        return cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = CacheTtl;
            return await LoadAsync(scoped, cancellationToken);
        })!;
    }

    private async Task<GpsIntegrationDashboardVm> LoadAsync(Guid scoped, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var since = now.AddHours(-24);

        var operators = await Context.Operators.Where(o => o.AccountId == scoped)
            .Select(o => new { o.Enabled, o.HealthStatus, o.LastSuccessfulSyncAt, o.LastManualSyncAt })
            .ToListAsync(cancellationToken);

        var deviceStats = await Context.Devices
            .Where(d => d.Operator!.AccountId == scoped)
            .Select(d => new
            {
                Status = d.DetectedStatus == (int)DetectedStatus.Ignored
                    ? (int)DetectedStatus.Ignored
                    : d.DetectedStatus == (int)DetectedStatus.Removed
                        ? (int)DetectedStatus.Removed
                        : d.Assignments.Any(a => a.Status == (int)AssignmentStatus.Active)
                            ? (int)DetectedStatus.Assigned
                            : (int)DetectedStatus.Available
            })
            .GroupBy(d => d.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        var deviceCountsByProviderStatus = await Context.Devices
            .Where(d => d.Operator!.AccountId == scoped)
            .Select(d => new
            {
                d.OperatorId,
                OperatorName = d.Operator!.Name,
                Status = d.DetectedStatus == (int)DetectedStatus.Ignored
                    ? (int)DetectedStatus.Ignored
                    : d.DetectedStatus == (int)DetectedStatus.Removed
                        ? (int)DetectedStatus.Removed
                        : d.Assignments.Any(a => a.Status == (int)AssignmentStatus.Active)
                            ? (int)DetectedStatus.Assigned
                            : (int)DetectedStatus.Available
            })
            .GroupBy(d => new { d.OperatorId, d.OperatorName, d.Status })
            .Select(g => new DeviceProviderStatusCountVm(
                g.Key.OperatorId,
                g.Key.OperatorName,
                (DetectedStatus)g.Key.Status,
                g.Count()))
            .ToListAsync(cancellationToken);

        var devicesTotal = deviceStats.Sum(s => s.Count);
        int CountStatus(DetectedStatus s) => deviceStats.FirstOrDefault(x => x.Status == (int)s)?.Count ?? 0;

        var recentDevices = await Context.Devices
            .Where(d => d.Operator!.AccountId == scoped && d.FirstSeenAt >= since)
            .CountAsync(cancellationToken);

        var unassigned = await Context.Devices
            .Where(d => d.Operator!.AccountId == scoped
                && d.DetectedStatus != (int)DetectedStatus.Ignored
                && d.DetectedStatus != (int)DetectedStatus.Removed
                && !d.Assignments.Any(a => a.Status == (int)AssignmentStatus.Active))
            .CountAsync(cancellationToken);

        var syncRuns = await Context.OperatorSyncRuns
            .Where(r => r.AccountId == scoped && r.StartedAt >= since && r.CompletedAt != null)
            .Select(r => new { r.Result, Duration = (double?)(r.CompletedAt!.Value - r.StartedAt).TotalSeconds, r.TriggerType, r.StartedAt })
            .ToListAsync(cancellationToken);

        var lastAuto = await Context.OperatorSyncRuns
            .Where(r => r.AccountId == scoped && r.TriggerType == (int)SyncTriggerType.Automatic)
            .OrderByDescending(r => r.StartedAt).Select(r => (DateTimeOffset?)r.StartedAt).FirstOrDefaultAsync(cancellationToken);
        var lastManual = await Context.OperatorSyncRuns
            .Where(r => r.AccountId == scoped && r.TriggerType == (int)SyncTriggerType.Manual)
            .OrderByDescending(r => r.StartedAt).Select(r => (DateTimeOffset?)r.StartedAt).FirstOrDefaultAsync(cancellationToken);

        return new GpsIntegrationDashboardVm(
            OperatorsTotal: operators.Count,
            OperatorsEnabled: operators.Count(o => o.Enabled),
            OperatorsHealthy: operators.Count(o => o.HealthStatus == (int)OperatorHealthStatus.Healthy),
            OperatorsDegraded: operators.Count(o => o.HealthStatus == (int)OperatorHealthStatus.Degraded),
            OperatorsOffline: operators.Count(o => o.HealthStatus == (int)OperatorHealthStatus.Offline),
            DevicesTotal: devicesTotal,
            DevicesNew: CountStatus(DetectedStatus.New),
            DevicesAvailable: CountStatus(DetectedStatus.Available),
            DevicesAssigned: CountStatus(DetectedStatus.Assigned),
            DevicesIgnored: CountStatus(DetectedStatus.Ignored),
            DevicesRemoved: CountStatus(DetectedStatus.Removed),
            RecentlyAddedDevicesLast24h: recentDevices,
            UnassignedDevicesCount: unassigned,
            SyncRunsSucceededLast24h: syncRuns.Count(r => r.Result == (int)OperatorSyncResult.Succeeded || r.Result == (int)OperatorSyncResult.PartiallySucceeded),
            SyncRunsFailedLast24h: syncRuns.Count(r => r.Result == (int)OperatorSyncResult.Failed),
            LastAutomaticSyncAt: lastAuto,
            LastManualSyncAt: lastManual,
            AverageSyncDurationSeconds: syncRuns.Count > 0 ? syncRuns.Average(r => r.Duration ?? 0) : 0,
            DeviceCountsByProviderStatus: deviceCountsByProviderStatus);
    }
}
