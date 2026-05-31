namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.PositionHistory, Action = Actions.Read)]

public readonly record struct GetPositionRetentionPolicyQuery(Guid AccountId) : IRequest<PositionRetentionPolicyVm>;

public class GetPositionRetentionPolicyQueryHandler(IPositionRetentionPolicyReader reader)
    : IRequestHandler<GetPositionRetentionPolicyQuery, PositionRetentionPolicyVm>
{
    public Task<PositionRetentionPolicyVm> Handle(GetPositionRetentionPolicyQuery request, CancellationToken cancellationToken)
        => reader.GetAsync(request.AccountId, cancellationToken);
}
