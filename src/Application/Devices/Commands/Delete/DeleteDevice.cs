namespace TrackHub.Manager.Application.Devices.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public record DeleteDeviceCommand(Guid Id) : IRequest;

public class DeleteDeviceCommandHandler(IDeviceWriter writer) : IRequestHandler<DeleteDeviceCommand>
{
    public async Task Handle(DeleteDeviceCommand request, CancellationToken cancellationToken)
        => await writer.DeleteDeviceAsync(request.Id, cancellationToken);
}
