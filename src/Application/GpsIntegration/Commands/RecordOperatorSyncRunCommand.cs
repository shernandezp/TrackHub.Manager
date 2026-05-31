namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.OperatorSyncRuns, Action = Actions.Write, PrincipalTypes = "User,ServiceClient")]
public readonly record struct RecordOperatorSyncRunCommand(OperatorSyncRunDto Run) : IRequest<OperatorSyncRunVm>;

public class RecordOperatorSyncRunCommandHandler(IOperatorSyncRunWriter writer, IOperatorWriter operatorWriter)
    : IRequestHandler<RecordOperatorSyncRunCommand, OperatorSyncRunVm>
{
    public async Task<OperatorSyncRunVm> Handle(RecordOperatorSyncRunCommand request, CancellationToken cancellationToken)
    {
        var vm = await writer.RecordAsync(request.Run, cancellationToken);
        if (request.Run.CompletedAt.HasValue)
        {
            var success = request.Run.Result is OperatorSyncResult.Succeeded or OperatorSyncResult.PartiallySucceeded;
            await operatorWriter.UpdateSyncSummaryAsync(
                request.Run.OperatorId, success, request.Run.CompletedAt.Value,
                request.Run.TriggerType,
                deviceSync: request.Run.DevicesSeen > 0,
                positionSync: request.Run.PositionsRead > 0,
                request.Run.ErrorCode, request.Run.ErrorMessage, cancellationToken);
        }
        return vm;
    }
}
