namespace TrackHub.Manager.Application.Devices.Queries.GetByGroup;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByGroupByOperatorQuery(long GroupId, Guid OperatorId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDeviceByGroupByOperatorQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceByGroupByOperatorQuery, IReadOnlyCollection<DeviceVm>>
{
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByGroupByOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByGroupAsync(request.GroupId, request.OperatorId, cancellationToken);

}
