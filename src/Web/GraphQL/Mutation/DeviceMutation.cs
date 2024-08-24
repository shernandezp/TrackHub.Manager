using TrackHub.Manager.Application.Device.Commands.Process;
using TrackHub.Manager.Application.Device.Commands.Delete;
using TrackHub.Manager.Application.Device.Commands.Wipe;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<bool> ProcessDevice([Service] ISender sender, ProcessDeviceCommand command)
        => await sender.Send(command);

    public async Task<Guid> WipeDevices([Service] ISender sender, Guid operatorId)
    {
        await sender.Send(new WipeDevicesCommand(operatorId));
        return operatorId;
    }

    public async Task<Guid> DeleteDevice([Service] ISender sender, Guid deviceId)
    {
        await sender.Send(new DeleteDeviceCommand(deviceId));
        return deviceId;
    }
}
