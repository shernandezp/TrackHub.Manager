namespace TrackHub.Manager.Application.DeviceOperator.Queries.Get;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceOperatorQuery(long Id) : IRequest<DeviceOperatorVm>;

public class GetDevicesOperatorQueryHandler(IDeviceOperatorReader reader) : IRequestHandler<GetDeviceOperatorQuery, DeviceOperatorVm>
{
    public async Task<DeviceOperatorVm> Handle(GetDeviceOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetDeviceOperatorAsync(request.Id, cancellationToken);

}
