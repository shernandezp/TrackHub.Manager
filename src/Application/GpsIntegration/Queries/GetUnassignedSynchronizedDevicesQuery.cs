namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Read)]

public readonly record struct GetUnassignedSynchronizedDevicesQuery(Guid AccountId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetUnassignedSynchronizedDevicesQueryHandler(IDeviceReader reader)
    : IRequestHandler<GetUnassignedSynchronizedDevicesQuery, IReadOnlyCollection<DeviceVm>>
{
    public Task<IReadOnlyCollection<DeviceVm>> Handle(GetUnassignedSynchronizedDevicesQuery request, CancellationToken cancellationToken)
        => reader.GetUnassignedDevicesAsync(request.AccountId, cancellationToken);
}
