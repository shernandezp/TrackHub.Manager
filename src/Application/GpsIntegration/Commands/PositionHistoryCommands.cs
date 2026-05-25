using Common.Application.Attributes;
using Common.Domain.Constants;

namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.PositionHistory, Action = Actions.Edit)]
[RequireFeature(FeatureKeys.GpsPositionHistory)]
public readonly record struct SetPositionRetentionPolicyCommand(Guid AccountId, PositionRetentionPolicyDto Policy) : IRequest;
public class SetPositionRetentionPolicyCommandHandler(IPositionRetentionPolicyWriter writer)
    : IRequestHandler<SetPositionRetentionPolicyCommand>
{
    public Task Handle(SetPositionRetentionPolicyCommand request, CancellationToken cancellationToken)
        => writer.SetAsync(request.AccountId, request.Policy, cancellationToken);
}
