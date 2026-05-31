namespace TrackHub.Manager.Domain.Interfaces;

public interface ISyncDispatcher
{
    Task<bool> DispatchManualSyncAsync(Guid accountId, Guid operatorId, bool resetDeviceCatalog, bool? autoAssignNewDevices, CancellationToken cancellationToken);
}
