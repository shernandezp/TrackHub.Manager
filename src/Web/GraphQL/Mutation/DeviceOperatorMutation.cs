using TrackHub.Manager.Application.Device.Commands.Create;
using TrackHub.Manager.Application.Device.Commands.Delete;
using TrackHub.Manager.Application.Device.Commands.Update;

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

    public async Task<Guid> DeleteDevice([Service] ISender sender, Guid deviceId)
    {
        await sender.Send(new DeleteDeviceCommand(deviceId));
        return deviceId;
    }
}
