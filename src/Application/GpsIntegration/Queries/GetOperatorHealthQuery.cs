namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.OperatorHealth, Action = Actions.Read)]
[RequireFeature(FeatureKeys.GpsIntegration)]
public readonly record struct GetOperatorHealthQuery(Guid OperatorId) : IRequest<OperatorHealthVm>;

public class GetOperatorHealthQueryHandler(IOperatorHealthCheckReader reader) : IRequestHandler<GetOperatorHealthQuery, OperatorHealthVm>
{
    public Task<OperatorHealthVm> Handle(GetOperatorHealthQuery request, CancellationToken cancellationToken)
        => reader.GetLatestHealthAsync(request.OperatorId, cancellationToken);
}
