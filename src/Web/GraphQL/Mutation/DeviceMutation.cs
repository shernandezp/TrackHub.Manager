using TrackHub.Manager.Application.Devices.Commands.Create;
using TrackHub.Manager.Application.Devices.Commands.Delete;
using TrackHub.Manager.Application.Devices.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DeviceVm> CreateDevice([Service] ISender sender, CreateDeviceCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateDevice([Service] ISender sender, Guid id, UpdateDeviceCommand command)
    {
        if (id != command.Device.DeviceId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteDevice([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteDeviceCommand(id));
        return id;
    }
}
