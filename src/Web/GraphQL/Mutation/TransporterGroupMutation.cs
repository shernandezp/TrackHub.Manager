using TrackHub.Manager.Application.TransporterGroup.Commands.Create;
using TrackHub.Manager.Application.TransporterGroup.Commands.Delete;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<TransporterGroupVm> CreateTransporterGroup([Service] ISender sender, CreateTransporterGroupCommand command)
        => await sender.Send(command);

    public async Task<Guid> DeleteTransporterGroup([Service] ISender sender, Guid transporterId, long groupId)
    {
        await sender.Send(new DeleteTransporterGroupCommand(transporterId, groupId));
        return transporterId;
    }
}
