namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.OperatorHealth, Action = Actions.Read)]
[RequireFeature(FeatureKeys.GpsIntegration)]
public readonly record struct GetOperatorHealthHistoryQuery(Guid OperatorId, int Take = 50) : IRequest<IReadOnlyCollection<OperatorHealthCheckVm>>;

public class GetOperatorHealthHistoryQueryHandler(IOperatorHealthCheckReader reader)
    : IRequestHandler<GetOperatorHealthHistoryQuery, IReadOnlyCollection<OperatorHealthCheckVm>>
{
    public Task<IReadOnlyCollection<OperatorHealthCheckVm>> Handle(GetOperatorHealthHistoryQuery request, CancellationToken cancellationToken)
        => reader.GetByOperatorAsync(request.OperatorId, request.Take, cancellationToken);
}
