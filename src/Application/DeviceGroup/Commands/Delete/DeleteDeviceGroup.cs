namespace TrackHub.Manager.Application.DeviceGroup.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct DeleteDeviceGroupCommand(Guid DeviceId, long GroupId) : IRequest;

public class DeleteDeviceGroupCommandHandler(IDeviceGroupWriter writer) : IRequestHandler<DeleteDeviceGroupCommand>
{

    public async Task Handle(DeleteDeviceGroupCommand request, CancellationToken cancellationToken)
        => await writer.DeleteDeviceGroupAsync(request.DeviceId, request.GroupId, cancellationToken);

}
