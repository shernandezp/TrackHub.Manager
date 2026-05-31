using TrackHub.Manager.Application.AccountFeatures.Queries.Get;
using TrackHub.Manager.Application.AccountFeatures.Queries.Validate;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<AccountFeatureVm>> GetAccountFeatures([Service] ISender sender, [AsParameters] GetAccountFeaturesQuery query) => await sender.Send(query);
    public async Task<bool> ValidateFeatureEnabled([Service] ISender sender, [AsParameters] ValidateFeatureEnabledQuery query) => await sender.Send(query);
}
