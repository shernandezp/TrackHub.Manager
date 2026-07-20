using TrackHub.Manager.Application.PlatformStatus.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<PlatformAnnouncementVm>> GetPlatformAnnouncements([Service] ISender sender, [AsParameters] GetPlatformAnnouncementsQuery query) => await sender.Send(query);
    // No [AsParameters]: the query carries no arguments, and an empty GraphQL input object is invalid.
    public async Task<IReadOnlyCollection<BackgroundJobStatusVm>> GetBackgroundJobStatus([Service] ISender sender) => await sender.Send(new GetBackgroundJobStatusQuery());
}
