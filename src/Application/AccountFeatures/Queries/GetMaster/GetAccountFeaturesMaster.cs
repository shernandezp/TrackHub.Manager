namespace TrackHub.Manager.Application.AccountFeatures.Queries.GetMaster;

[Authorize(Resource = Resources.AccountFeaturesMaster, Action = Actions.Read)]
public readonly record struct GetAccountFeaturesMasterQuery(Guid AccountId) : IRequest<IReadOnlyCollection<AccountFeatureVm>>;

public class GetAccountFeaturesMasterQueryHandler(IAccountFeatureMasterReader reader) : IRequestHandler<GetAccountFeaturesMasterQuery, IReadOnlyCollection<AccountFeatureVm>>
{
    public async Task<IReadOnlyCollection<AccountFeatureVm>> Handle(GetAccountFeaturesMasterQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountFeaturesAsync(request.AccountId, cancellationToken);
}
