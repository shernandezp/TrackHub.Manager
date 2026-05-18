using TrackHub.Manager.Application.PublicLinks.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<PublicLinkGrantVm> GetPublicLinkGrant([Service] ISender sender, [AsParameters] GetPublicLinkGrantQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<PublicLinkGrantVm>> GetPublicLinkGrantsByAccount([Service] ISender sender, [AsParameters] GetPublicLinkGrantsByAccountQuery query) => await sender.Send(query);
}
