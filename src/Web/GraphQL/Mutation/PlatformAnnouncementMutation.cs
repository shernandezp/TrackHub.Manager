using TrackHub.Manager.Application.PlatformStatus.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<PlatformAnnouncementVm> CreatePlatformAnnouncement([Service] ISender sender, CreatePlatformAnnouncementCommand command) => await sender.Send(command);
    public async Task<bool> UpdatePlatformAnnouncement([Service] ISender sender, UpdatePlatformAnnouncementCommand command) { await sender.Send(command); return true; }
    public async Task<Guid> DeletePlatformAnnouncement([Service] ISender sender, DeletePlatformAnnouncementCommand command) => await sender.Send(command);
}
