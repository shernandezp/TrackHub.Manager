using TrackHub.Manager.Application.DeviceOperator.Commands.Create;
using TrackHub.Manager.Application.DeviceOperator.Commands.Delete;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DeviceOperatorVm> CreateDeviceOperator([Service] ISender sender, CreateDeviceOperatorCommand command)
        => await sender.Send(command);

    public async Task<Guid> DeleteDeviceOperator([Service] ISender sender, Guid deviceId, Guid operatorId)
    {
        await sender.Send(new DeleteDeviceOperatorCommand(deviceId, operatorId));
        return deviceId;
    }
}
