namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.OperatorHealth, Action = Actions.Read)]
[RequireFeature(FeatureKeys.GpsIntegration)]
public readonly record struct GetOperatorHealthSummaryQuery(Guid OperatorId, int LookbackHours = 24) : IRequest<OperatorHealthSummaryVm>;

public class GetOperatorHealthSummaryQueryHandler(IOperatorHealthCheckReader reader)
    : IRequestHandler<GetOperatorHealthSummaryQuery, OperatorHealthSummaryVm>
{
    public Task<OperatorHealthSummaryVm> Handle(GetOperatorHealthSummaryQuery request, CancellationToken cancellationToken)
    {
        var hours = request.LookbackHours <= 0 ? 24 : Math.Min(request.LookbackHours, 24 * 90);
        return reader.GetSummaryAsync(request.OperatorId, DateTimeOffset.UtcNow.AddHours(-hours), cancellationToken);
    }
}
