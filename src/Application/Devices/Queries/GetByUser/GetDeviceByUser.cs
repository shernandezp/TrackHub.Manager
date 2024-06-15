namespace TrackHub.Manager.Application.Devices.Queries.GetByUser;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByUserQuery(Guid UserId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceByUserQuery, IReadOnlyCollection<DeviceVm>>
{
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByUserAsync(request.UserId, cancellationToken);

}
