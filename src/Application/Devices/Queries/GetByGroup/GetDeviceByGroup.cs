namespace TrackHub.Manager.Application.Devices.Queries.GetByGroup;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByGroupQuery(long GroupId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceByGroupQuery, IReadOnlyCollection<DeviceVm>>
{
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByGroupQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByGroupAsync(request.GroupId, cancellationToken);

}
