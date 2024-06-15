namespace TrackHub.Manager.Application.Devices.Commands.Create;

[Authorize(Resource = Resources.Devices, Action = Actions.Write)]
public readonly record struct CreateDeviceCommand(DeviceDto Device) : IRequest<DeviceVm>;

public class CreateDeviceCommandHandler(IDeviceWriter writer) : IRequestHandler<CreateDeviceCommand, DeviceVm>
{
    public async Task<DeviceVm> Handle(CreateDeviceCommand request, CancellationToken cancellationToken)
        => await writer.CreateDeviceAsync(request.Device, cancellationToken);
}
