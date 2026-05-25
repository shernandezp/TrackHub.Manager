namespace TrackHub.Manager.Domain.Interfaces;

public interface ISyncDispatcher
{
    Task DispatchManualSyncAsync(Guid accountId, Guid operatorId, CancellationToken cancellationToken);
}
