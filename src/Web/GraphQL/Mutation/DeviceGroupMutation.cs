using TrackHub.Manager.Application.DeviceGroup.Commands.Create;
using TrackHub.Manager.Application.DeviceGroup.Commands.Delete;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DeviceGroupVm> CreateDeviceGroup([Service] ISender sender, CreateDeviceGroupCommand command)
        => await sender.Send(command);

    public async Task<Guid> DeleteDeviceGroup([Service] ISender sender, Guid deviceId, long GroupId)
    {
        await sender.Send(new DeleteDeviceGroupCommand(deviceId, GroupId));
        return deviceId;
    }
}
