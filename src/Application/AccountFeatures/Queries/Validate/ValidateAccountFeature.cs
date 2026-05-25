namespace TrackHub.Manager.Application.AccountFeatures.Queries.Validate;

public readonly record struct ValidateFeatureEnabledQuery(Guid AccountId, string FeatureKey) : IRequest<bool>;
public class ValidateFeatureEnabledQueryHandler(IAccountFeatureReader reader) : IRequestHandler<ValidateFeatureEnabledQuery, bool>
{
    public async Task<bool> Handle(ValidateFeatureEnabledQuery request, CancellationToken cancellationToken) => await reader.ValidateFeatureEnabledAsync(request.AccountId, request.FeatureKey, cancellationToken);
}
