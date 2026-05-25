namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.OperatorHealth, Action = Actions.Write, PrincipalTypes = "ServiceClient")]
[RequireFeature(FeatureKeys.GpsIntegration, AllowGlobalServiceClient = false)]
public readonly record struct RecordOperatorHealthCommand(OperatorHealthCheckDto Check) : IRequest<OperatorHealthCheckVm>;
public class RecordOperatorHealthCommandHandler(IOperatorHealthCheckWriter writer, IOperatorWriter operatorWriter)
    : IRequestHandler<RecordOperatorHealthCommand, OperatorHealthCheckVm>
{
    public async Task<OperatorHealthCheckVm> Handle(RecordOperatorHealthCommand request, CancellationToken cancellationToken)
    {
        var vm = await writer.RecordAsync(request.Check, cancellationToken);
        await operatorWriter.UpdateHealthSummaryAsync(
            request.Check.OperatorId,
            request.Check.Status,
            request.Check.CompletedAt ?? request.Check.StartedAt,
            request.Check.LatencyMs,
            request.Check.ErrorCode,
            request.Check.ErrorMessage,
            cancellationToken);
        return vm;
    }
}
