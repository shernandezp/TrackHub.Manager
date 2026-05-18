using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class AlertEventReader(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IAlertEventReader
{
    private static int PageSize(int take) => Math.Clamp(take <= 0 ? 50 : take, 1, 500);
    private static int Offset(int skip) => Math.Max(0, skip);

    public async Task<IReadOnlyCollection<AlertEventVm>> GetAlertEventsAsync(Guid accountId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken)
        => await Context.AlertEvents
            .Where(x => x.AccountId == RequireAccountAccess(accountId) && (!from.HasValue || x.LastSeenAt >= from) && (!to.HasValue || x.LastSeenAt <= to))
            .OrderByDescending(x => x.LastSeenAt).ThenBy(x => x.AlertEventId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new AlertEventVm(x.AlertEventId, x.AccountId, x.EventType, x.Severity, x.SourceModule, x.ResourceType, x.ResourceId, x.Status, x.FirstSeenAt, x.LastSeenAt, x.PayloadJson, x.DeduplicationKey, x.LastModified))
            .ToListAsync(cancellationToken);
}
