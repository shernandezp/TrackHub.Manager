using TrackHub.Manager.Application.Device.Events;

namespace TrackHub.Manager.Application.Device.Commands.Wipe;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct WipeDevicesCommand(Guid OperatorId) : IRequest;

public class WipeDevicesCommandHandler(
        IPublisher publisher,
        IDeviceWriter writer,
        IDeviceReader reader) : IRequestHandler<WipeDevicesCommand>
{
    // This method handles the execution of the WipeDevicesCommand.
    // It deletes all devices associated with the specified operator.
    public async Task Handle(WipeDevicesCommand request, CancellationToken cancellationToken)
    {
        var devices = await reader.GetDevicesByOperatorAsync(request.OperatorId, cancellationToken);
        foreach (var device in devices)
        {
            await writer.DeleteDeviceAsync(device.DeviceId, cancellationToken);
            await publisher.Publish(new DeviceDeleted.Notification(device.TransporterId, device.DeviceId), cancellationToken);
        }
    }
}
