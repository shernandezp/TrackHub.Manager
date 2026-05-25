using TrackHub.Manager.Domain.Interfaces;

namespace TrackHub.Manager.Application.GpsIntegration;

/// Default no-op dispatcher. The Router replaces this with an HTTP-based implementation that
/// signals the SyncWorker to run a manual cycle for the given operator.
public sealed class NoopSyncDispatcher : ISyncDispatcher
{
    public Task DispatchManualSyncAsync(Guid accountId, Guid operatorId, CancellationToken cancellationToken)
        => Task.CompletedTask;
}
