namespace TrackHub.Manager.Application.AccountFeatures.Commands;

[Authorize(Resource = Resources.AccountFeaturesMaster, Action = Actions.Write)]
public readonly record struct SetAccountFeatureMasterCommand(AccountFeatureDto Feature) : IRequest<AccountFeatureVm>;
public class SetAccountFeatureMasterCommandHandler(IAccountFeatureMasterWriter writer) : IRequestHandler<SetAccountFeatureMasterCommand, AccountFeatureVm>
{
    public async Task<AccountFeatureVm> Handle(SetAccountFeatureMasterCommand request, CancellationToken cancellationToken) => await writer.SetAccountFeatureAsync(request.Feature, cancellationToken);
}
