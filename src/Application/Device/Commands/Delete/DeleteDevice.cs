namespace TrackHub.Manager.Application.Device.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct DeleteDeviceCommand(Guid DeviceId) : IRequest;

public class DeleteDeviceCommandHandler(IDeviceWriter writer) : IRequestHandler<DeleteDeviceCommand>
{

    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        => await writer.DeleteDeviceAsync(request.DeviceId, cancellationToken);

}
