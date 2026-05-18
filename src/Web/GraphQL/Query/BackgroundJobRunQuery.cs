using TrackHub.Manager.Application.BackgroundJobs.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<BackgroundJobRunVm>> GetBackgroundJobRuns([Service] ISender sender, [AsParameters] GetBackgroundJobRunsQuery query) => await sender.Send(query);
}
