namespace TrackHub.Manager.Application.Device.Commands.Wipe;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct WipeDevicesCommand(Guid OperatorId) : IRequest;

public class WipeDevicesCommandHandler(IDeviceWriter writer) : IRequestHandler<WipeDevicesCommand>
{
    public async Task Handle(WipeDevicesCommand request, CancellationToken cancellationToken)
        => await writer.DeleteDevicesByOperatorAsync(request.OperatorId, cancellationToken);
}
