using TrackHub.Manager.Application.AlertEvents.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<AlertEventVm> RecordAlertEvent([Service] ISender sender, RecordAlertEventCommand command) => await sender.Send(command);
    public async Task<bool> AcknowledgeAlertEvent([Service] ISender sender, AcknowledgeAlertEventCommand command) { await sender.Send(command); return true; }
    public async Task<bool> ResolveAlertEvent([Service] ISender sender, ResolveAlertEventCommand command) { await sender.Send(command); return true; }
}
