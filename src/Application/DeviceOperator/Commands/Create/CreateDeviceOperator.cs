namespace TrackHub.Manager.Application.DeviceOperator.Commands.Create;

[Authorize(Resource = Resources.Devices, Action = Actions.Write)]
public readonly record struct CreateDeviceOperatorCommand(DeviceOperatorDto DeviceOperator) : IRequest<DeviceOperatorVm>;

public class CreateDeviceOperatorCommandHandler(IDeviceOperatorWriter writer) : IRequestHandler<CreateDeviceOperatorCommand, DeviceOperatorVm>
{
    public async Task<DeviceOperatorVm> Handle(CreateDeviceOperatorCommand request, CancellationToken cancellationToken)
        => await writer.CreateDeviceOperatorAsync(request.DeviceOperator, cancellationToken);
}
