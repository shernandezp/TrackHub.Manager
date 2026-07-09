using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class AlertEventWriter(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IAlertEventWriter
{
    public async Task<AlertEventVm> RecordAlertEventAsync(AlertEventDto alertEvent, CancellationToken cancellationToken)
    {
        var accountId = RequireAccountWriteAccess(alertEvent.AccountId);
        var entity = await Context.AlertEvents.FirstOrDefaultAsync(x => x.AccountId == accountId && x.DeduplicationKey == alertEvent.DeduplicationKey && x.Status != "Resolved", cancellationToken);
        if (entity == null)
        {
            entity = new AlertEvent(accountId, alertEvent.EventType, alertEvent.Severity, alertEvent.SourceModule, alertEvent.ResourceType, alertEvent.ResourceId, alertEvent.Status, alertEvent.PayloadJson, alertEvent.DeduplicationKey);
            await Context.AlertEvents.AddAsync(entity, cancellationToken);
        }
        else
        {
            Context.AlertEvents.Attach(entity);
            entity.LastSeenAt = DateTimeOffset.UtcNow;
            entity.PayloadJson = alertEvent.PayloadJson;
        }

        await Context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task AcknowledgeAlertEventAsync(Guid alertEventId, CancellationToken cancellationToken) => await UpdateStatusAsync(alertEventId, "Acknowledged", cancellationToken);
    public async Task ResolveAlertEventAsync(Guid alertEventId, CancellationToken cancellationToken) => await UpdateStatusAsync(alertEventId, "Resolved", cancellationToken);

    private async Task UpdateStatusAsync(Guid alertEventId, string status, CancellationToken cancellationToken)
    {
        var entity = await Context.AlertEvents.FirstAsync(x => x.AlertEventId == alertEventId, cancellationToken);
        RequireAccountWriteAccess(entity.AccountId);
        Context.AlertEvents.Attach(entity);
        entity.Status = status;
        await Context.SaveChangesAsync(cancellationToken);
    }

    private static AlertEventVm ToVm(AlertEvent x) => new(x.AlertEventId, x.AccountId, x.EventType, x.Severity, x.SourceModule, x.ResourceType, x.ResourceId, x.Status, x.FirstSeenAt, x.LastSeenAt, x.PayloadJson, x.DeduplicationKey, x.LastModified);
}
