namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.PositionHistory, Action = Actions.Write, PrincipalTypes = "ServiceClient")]
[RequireFeature(FeatureKeys.GpsPositionHistory, AllowGlobalServiceClient = false)]
public readonly record struct AppendPositionHistoryCommand(TransporterPositionHistoryDto Position) : IRequest<bool>;

public class AppendPositionHistoryCommandHandler(ITransporterPositionHistoryWriter writer, IPositionRetentionPolicyReader policyReader)
    : IRequestHandler<AppendPositionHistoryCommand, bool>
{
    public async Task<bool> Handle(AppendPositionHistoryCommand request, CancellationToken cancellationToken)
    {
        var policy = await policyReader.GetAsync(request.Position.AccountId, cancellationToken);
        if (!policy.HistoryEnabled)
            return false;
        return await writer.AppendAsync(request.Position, cancellationToken);
    }
}
