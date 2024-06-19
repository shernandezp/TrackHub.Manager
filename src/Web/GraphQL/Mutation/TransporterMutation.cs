using TrackHub.Manager.Application.Transporters.Commands.Create;
using TrackHub.Manager.Application.Transporters.Commands.Delete;
using TrackHub.Manager.Application.Transporters.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<TransporterVm> CreateTransporter([Service] ISender sender, CreateTransporterCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateTransporter([Service] ISender sender, Guid id, UpdateTransporterCommand command)
    {
        if (id != command.Transporter.TransporterId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteTransporter([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteTransporterCommand(id));
        return id;
    }
}
