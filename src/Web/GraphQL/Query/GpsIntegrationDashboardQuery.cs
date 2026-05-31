using TrackHub.Manager.Application.GpsIntegration.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<GpsIntegrationDashboardVm> GetGpsIntegrationDashboard([Service] ISender sender, [AsParameters] GetGpsIntegrationDashboardQuery query)
        => await sender.Send(query);
}
