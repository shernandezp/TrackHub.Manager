using TrackHub.Manager.Application.GpsIntegration.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<OperatorSyncRunVm>> GetOperatorSyncRuns([Service] ISender sender, [AsParameters] GetOperatorSyncRunsQuery query)
        => await sender.Send(query);
}
