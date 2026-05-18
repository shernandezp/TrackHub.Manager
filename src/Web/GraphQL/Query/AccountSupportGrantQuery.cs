using TrackHub.Manager.Application.AccountSupportGrants.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<AccountSupportGrantVm> GetSupportGrantStatus([Service] ISender sender, [AsParameters] GetSupportGrantStatusQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<AccountSupportGrantVm>> GetAccountSupportGrants([Service] ISender sender, [AsParameters] GetAccountSupportGrantsQuery query) => await sender.Send(query);
}
