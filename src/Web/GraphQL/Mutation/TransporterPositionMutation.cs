using TrackHub.Manager.Application.TransporterPosition.Commands.Create;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<bool> BulkTransporterPosition([Service] ISender sender, BulkTransporterPositionCommand command)
    { 
        await sender.Send(command);
        return true;
    }

}
