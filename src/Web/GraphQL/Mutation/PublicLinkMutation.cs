using TrackHub.Manager.Application.PublicLinks.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<PublicLinkGrantVm> CreatePublicLinkGrant([Service] ISender sender, CreatePublicLinkGrantCommand command) => await sender.Send(command);
    public async Task<bool> RevokePublicLinkGrant([Service] ISender sender, RevokePublicLinkGrantCommand command) { await sender.Send(command); return true; }
    // ServiceClient-only resolution for downstream services (TripManagement's public tracking page).
    // Shares one implementation with the anonymous REST endpoint, so access counting and the
    // `PublicLinkAccessed` audit event happen exactly once per successful resolution.
    // Replaces the removed `recordPublicLinkAccess` mutation (spec 11 §7.8/§18.10).
    public async Task<PublicLinkResolutionVm> ResolvePublicLinkGrant([Service] ISender sender, ResolvePublicLinkGrantCommand command) => await sender.Send(command);
}
