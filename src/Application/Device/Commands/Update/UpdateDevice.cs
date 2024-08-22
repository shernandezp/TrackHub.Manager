namespace TrackHub.Manager.Application.Device.Commands.Update;

[Authorize(Resource = Resources.Devices, Action = Actions.Edit)]
public readonly record struct UpdateDeviceCommand(UpdateDeviceDto Device) : IRequest;

public class UpdateDeviceCommandHandler(IDeviceWriter writer) : IRequestHandler<UpdateDeviceCommand>
{
    public async Task Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
        => await writer.UpdateDeviceAsync(request.Device, cancellationToken);
}
