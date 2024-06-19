namespace TrackHub.Manager.Application.DeviceGroup.Commands.Create;

[Authorize(Resource = Resources.Devices, Action = Actions.Write)]
public readonly record struct CreateDeviceGroupCommand(DeviceGroupDto DeviceGroup) : IRequest<DeviceGroupVm>;

public class CreateDeviceGroupCommandHandler(IDeviceGroupWriter writer) : IRequestHandler<CreateDeviceGroupCommand, DeviceGroupVm>
{
    public async Task<DeviceGroupVm> Handle(CreateDeviceGroupCommand request, CancellationToken cancellationToken)
        => await writer.CreateDeviceGroupAsync(request.DeviceGroup, cancellationToken);
}
