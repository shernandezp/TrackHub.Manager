namespace TrackHub.Manager.Application.DeviceOperator.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct DeleteDeviceOperatorCommand(Guid DeviceId, Guid OperatorId) : IRequest;

public class DeleteDeviceOperatorCommandHandler(IDeviceOperatorWriter writer) : IRequestHandler<DeleteDeviceOperatorCommand>
{

    public async Task Handle(DeleteDeviceOperatorCommand request, CancellationToken cancellationToken)
        => await writer.DeleteDeviceOperatorAsync(request.DeviceId, request.OperatorId, cancellationToken);

}
