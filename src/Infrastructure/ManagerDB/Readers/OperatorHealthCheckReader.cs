using Common.Application.Interfaces;
using TrackHub.Manager.Domain.Enums;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class OperatorHealthCheckReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IOperatorHealthCheckReader
{
    public async Task<IReadOnlyCollection<OperatorHealthCheckVm>> GetByOperatorAsync(Guid operatorId, int take, CancellationToken cancellationToken)
    {
        var op = await Context.Operators.Where(o => o.OperatorId == operatorId)
            .Select(o => new { o.AccountId }).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Operator", $"{operatorId}");
        RequireAccountAccess(op.AccountId);
        var pageSize = Math.Clamp(take <= 0 ? 50 : take, 1, 500);
        return await Context.OperatorHealthChecks
            .Where(c => c.OperatorId == operatorId)
            .OrderByDescending(c => c.StartedAt)
            .Take(pageSize)
            .Select(c => new OperatorHealthCheckVm(c.OperatorHealthCheckId, c.AccountId, c.OperatorId,
                (OperatorHealthCheckType)c.CheckType, (OperatorHealthStatus)c.Status, c.LatencyMs,
                c.StartedAt, c.CompletedAt, c.ErrorCode, c.ErrorMessage, c.RetryCount, c.CorrelationId))
            .ToListAsync(cancellationToken);
    }

    public async Task<OperatorHealthSummaryVm> GetSummaryAsync(Guid operatorId, DateTimeOffset since, CancellationToken cancellationToken)
    {
        var op = await Context.Operators.Where(o => o.OperatorId == operatorId)
            .Select(o => new { o.AccountId }).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Operator", $"{operatorId}");
        RequireAccountAccess(op.AccountId);

        var checks = await Context.OperatorHealthChecks
            .Where(c => c.OperatorId == operatorId && c.StartedAt >= since)
            .Select(c => new { c.Status, c.LatencyMs, c.StartedAt, c.ErrorCode })
            .ToListAsync(cancellationToken);

        var total = checks.Count;
        var healthy = checks.Count(c => c.Status == (int)OperatorHealthStatus.Healthy);
        var degraded = checks.Count(c => c.Status == (int)OperatorHealthStatus.Degraded);
        var offline = checks.Count(c => c.Status == (int)OperatorHealthStatus.Offline);
        var failures = degraded + offline;
        var uptime = total == 0 ? 0d : Math.Round(100d * healthy / total, 2);
        var avgLatency = checks.Where(c => c.LatencyMs.HasValue).Select(c => (double)c.LatencyMs!.Value).DefaultIfEmpty().Average();
        var hasLatency = checks.Any(c => c.LatencyMs.HasValue);
        var last = checks.OrderByDescending(c => c.StartedAt).FirstOrDefault();
        var lastFailure = checks.Where(c => c.Status != (int)OperatorHealthStatus.Healthy)
            .OrderByDescending(c => c.StartedAt).FirstOrDefault();

        return new OperatorHealthSummaryVm(
            operatorId,
            since,
            total,
            healthy,
            degraded,
            offline,
            failures,
            uptime,
            hasLatency ? avgLatency : null,
            last is null ? null : last.StartedAt,
            lastFailure is null ? null : lastFailure.StartedAt,
            lastFailure?.ErrorCode);
    }

    public async Task<DateTimeOffset?> GetLastCheckAtAsync(Guid operatorId, CancellationToken cancellationToken)
        => await Context.OperatorHealthChecks
            .Where(c => c.OperatorId == operatorId)
            .OrderByDescending(c => c.StartedAt)
            .Select(c => (DateTimeOffset?)c.StartedAt)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<OperatorHealthVm> GetLatestHealthAsync(Guid operatorId, CancellationToken cancellationToken)
    {
        var op = await Context.Operators.Where(o => o.OperatorId == operatorId)
            .Select(o => new { o.AccountId, o.OperatorId, o.HealthStatus, o.LastSuccessfulSyncAt, o.LastFailedSyncAt, o.LastDeviceSyncAt, o.LastPositionSyncAt, o.LastFailureCode, o.LastFailureMessage, o.LastLatencyMs })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Operator", $"{operatorId}");
        RequireAccountAccess(op.AccountId);
        return new OperatorHealthVm(op.OperatorId, (OperatorHealthStatus)op.HealthStatus,
            op.LastSuccessfulSyncAt, op.LastFailedSyncAt, op.LastDeviceSyncAt, op.LastPositionSyncAt,
            op.LastFailureCode, op.LastFailureMessage, op.LastLatencyMs);
    }
}
