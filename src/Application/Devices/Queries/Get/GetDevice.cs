namespace TrackHub.Manager.Application.Devices.Queries.Get;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceQuery(Guid Id) : IRequest<DeviceVm>;

public class GetDevicesQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceQuery, DeviceVm>
{
    public async Task<DeviceVm> Handle(GetDeviceQuery request, CancellationToken cancellationToken)
        => await reader.GetDeviceAsync(request.Id, cancellationToken);

}
