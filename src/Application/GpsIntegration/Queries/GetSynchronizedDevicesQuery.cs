using Common.Domain.Helpers;

namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Read)]

public readonly record struct GetSynchronizedDevicesQuery(
    Guid AccountId,
    DetectedStatus? DetectedStatus = null,
    Guid? OperatorId = null) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetSynchronizedDevicesQueryHandler(IDeviceReader reader)
    : IRequestHandler<GetSynchronizedDevicesQuery, IReadOnlyCollection<DeviceVm>>
{
    public Task<IReadOnlyCollection<DeviceVm>> Handle(GetSynchronizedDevicesQuery request, CancellationToken cancellationToken)
    {
        var dict = new Dictionary<string, object>();
        if (request.DetectedStatus.HasValue) dict[nameof(DeviceVm.DetectedStatus)] = request.DetectedStatus.Value;
        if (request.OperatorId.HasValue) dict[nameof(DeviceVm.OperatorId)] = request.OperatorId.Value;
        return reader.SearchDevicesAsync(request.AccountId, new Filters(dict), cancellationToken);
    }
}
