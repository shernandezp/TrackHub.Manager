using TrackHub.Manager.Application.Principals.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<CurrentPrincipalVm> GetCurrentPrincipal([Service] ISender sender) => await sender.Send(new GetCurrentPrincipalQuery());
}
