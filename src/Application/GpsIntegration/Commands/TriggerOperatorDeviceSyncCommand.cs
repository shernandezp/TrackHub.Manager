using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Execute)]
[RequireFeature(FeatureKeys.GpsIntegration)]
public readonly record struct TriggerOperatorDeviceSyncCommand(Guid OperatorId) : IRequest;

public class TriggerOperatorDeviceSyncCommandHandler(
    IOperatorSyncRunWriter syncWriter,
    IOperatorReader operatorReader,
    IOperatorWriter operatorWriter,
    ISyncDispatcher dispatcher,
    IConfiguration configuration,
    ILogger<TriggerOperatorDeviceSyncCommandHandler> logger)
    : IRequestHandler<TriggerOperatorDeviceSyncCommand>
{
    public async Task Handle(TriggerOperatorDeviceSyncCommand request, CancellationToken cancellationToken)
    {
        var op = await operatorReader.GetOperatorAsync(request.OperatorId, cancellationToken);

        var minIntervalSeconds = int.TryParse(configuration["GpsIntegration:ManualSyncMinIntervalSeconds"], out var v) ? v : 60;
        if (minIntervalSeconds > 0 && op.LastManualSyncAt is { } last)
        {
            var elapsed = DateTimeOffset.UtcNow - last;
            if (elapsed < TimeSpan.FromSeconds(minIntervalSeconds))
            {
                logger.LogInformation(
                    "Manual sync for operator {OperatorId} throttled; last trigger {Elapsed}s ago (min {Min}s).",
                    request.OperatorId, (int)elapsed.TotalSeconds, minIntervalSeconds);
                throw new Common.Application.Exceptions.TooManyRequestsException(
                    $"Manual sync throttled. Wait {minIntervalSeconds - (int)elapsed.TotalSeconds}s before retrying.");
            }
        }

        var now = DateTimeOffset.UtcNow;
        await operatorWriter.MarkManualSyncTriggeredAsync(request.OperatorId, now, cancellationToken);

        var correlationId = Guid.NewGuid().ToString();
        await syncWriter.RecordAsync(new OperatorSyncRunDto(
            op.AccountId, request.OperatorId, SyncTriggerType.Manual, OperatorSyncResult.Pending,
            now, null, 0, 0, 0, 0, 0, 0, 0, 0, null, null, correlationId), cancellationToken);
        await dispatcher.DispatchManualSyncAsync(op.AccountId, request.OperatorId, cancellationToken);
    }
}
