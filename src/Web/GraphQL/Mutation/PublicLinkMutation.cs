using TrackHub.Manager.Application.PublicLinks.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<PublicLinkGrantVm> CreatePublicLinkGrant([Service] ISender sender, CreatePublicLinkGrantCommand command) => await sender.Send(command);
    public async Task<bool> RevokePublicLinkGrant([Service] ISender sender, RevokePublicLinkGrantCommand command) { await sender.Send(command); return true; }
    public async Task<bool> RecordPublicLinkAccess([Service] ISender sender, RecordPublicLinkAccessCommand command) { await sender.Send(command); return true; }
}
