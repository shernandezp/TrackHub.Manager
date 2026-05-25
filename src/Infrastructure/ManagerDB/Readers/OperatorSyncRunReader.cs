using Common.Application.Interfaces;
using Common.Domain.Helpers;
using TrackHub.Manager.Domain.Enums;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class OperatorSyncRunReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IOperatorSyncRunReader
{
    public async Task<IReadOnlyCollection<OperatorSyncRunVm>> GetAsync(Filters filters, int take, CancellationToken cancellationToken)
    {
        var pageSize = Math.Clamp(take <= 0 ? 50 : take, 1, 500);
        var q = Context.OperatorSyncRuns.AsQueryable();
        q = filters.Apply(q);
        if (!CanAccessAllAccounts && Principal.AccountId.HasValue)
        {
            var acct = Principal.AccountId.Value;
            q = q.Where(x => x.AccountId == acct);
        }
        return await q.OrderByDescending(x => x.StartedAt)
            .Take(pageSize)
            .Select(x => new OperatorSyncRunVm(x.OperatorSyncRunId, x.AccountId, x.OperatorId,
                (SyncTriggerType)x.TriggerType, (OperatorSyncResult)x.Result, x.StartedAt, x.CompletedAt,
                x.DevicesSeen, x.DevicesAdded, x.DevicesUpdated, x.DevicesRemoved, x.DevicesIgnored,
                x.PositionsRead, x.PositionsAccepted, x.PositionsRejected, x.ErrorCode, x.ErrorMessage, x.CorrelationId))
            .ToListAsync(cancellationToken);
    }

    public async Task<OperatorSyncRunVm?> GetLatestAsync(Guid operatorId, CancellationToken cancellationToken)
    {
        var op = await Context.Operators.Where(o => o.OperatorId == operatorId)
            .Select(o => new { o.AccountId }).FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Operator", $"{operatorId}");
        RequireAccountAccess(op.AccountId);
        var x = await Context.OperatorSyncRuns.Where(x => x.OperatorId == operatorId)
            .OrderByDescending(x => x.StartedAt).FirstOrDefaultAsync(cancellationToken);
        if (x is null) return null;
        return new OperatorSyncRunVm(x.OperatorSyncRunId, x.AccountId, x.OperatorId,
            (SyncTriggerType)x.TriggerType, (OperatorSyncResult)x.Result, x.StartedAt, x.CompletedAt,
            x.DevicesSeen, x.DevicesAdded, x.DevicesUpdated, x.DevicesRemoved, x.DevicesIgnored,
            x.PositionsRead, x.PositionsAccepted, x.PositionsRejected, x.ErrorCode, x.ErrorMessage, x.CorrelationId);
    }
}
