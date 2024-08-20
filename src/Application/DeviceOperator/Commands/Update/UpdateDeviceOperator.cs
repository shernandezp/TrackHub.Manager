namespace TrackHub.Manager.Application.DeviceOperator.Commands.Update;

[Authorize(Resource = Resources.Devices, Action = Actions.Edit)]
public readonly record struct UpdateDeviceOperatorCommand(UpdateDeviceOperatorDto DeviceOperator) : IRequest;

public class UpdateDeviceOperatorCommandHandler(IDeviceOperatorWriter writer) : IRequestHandler<UpdateDeviceOperatorCommand>
{
    public async Task Handle(UpdateDeviceOperatorCommand request, CancellationToken cancellationToken)
        => await writer.UpdateDeviceOperatorAsync(request.DeviceOperator, cancellationToken);
}
