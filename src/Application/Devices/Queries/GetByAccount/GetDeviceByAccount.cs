namespace TrackHub.Manager.Application.Devices.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByAccountQuery(Guid AccountId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceByAccountQuery, IReadOnlyCollection<DeviceVm>>
{
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByAccountAsync(request.AccountId, cancellationToken);

}
