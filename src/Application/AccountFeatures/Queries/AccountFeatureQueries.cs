namespace TrackHub.Manager.Application.AccountFeatures.Queries;

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Read)]
public readonly record struct GetAccountFeaturesQuery(Guid AccountId) : IRequest<IReadOnlyCollection<AccountFeatureVm>>;
public class GetAccountFeaturesQueryHandler(IAccountFeatureReader reader) : IRequestHandler<GetAccountFeaturesQuery, IReadOnlyCollection<AccountFeatureVm>>
{
    public async Task<IReadOnlyCollection<AccountFeatureVm>> Handle(GetAccountFeaturesQuery request, CancellationToken cancellationToken) => await reader.GetAccountFeaturesAsync(request.AccountId, cancellationToken);
}

[Authorize(Resource = Resources.AccountFeatures, Action = Actions.Read)]
public readonly record struct ValidateFeatureEnabledQuery(Guid AccountId, string FeatureKey) : IRequest<bool>;
public class ValidateFeatureEnabledQueryHandler(IAccountFeatureReader reader) : IRequestHandler<ValidateFeatureEnabledQuery, bool>
{
    public async Task<bool> Handle(ValidateFeatureEnabledQuery request, CancellationToken cancellationToken) => await reader.ValidateFeatureEnabledAsync(request.AccountId, request.FeatureKey, cancellationToken);
}
