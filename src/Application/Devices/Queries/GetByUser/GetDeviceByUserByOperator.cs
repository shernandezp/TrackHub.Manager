namespace TrackHub.Manager.Application.Devices.Queries.GetByUser;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByUserByOperatorQuery(Guid UserId, Guid OperatorId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDeviceByUserByOperatorQueryHandler(IDeviceReader reader) : IRequestHandler<GetDeviceByUserByOperatorQuery, IReadOnlyCollection<DeviceVm>>
{
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByUserByOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByUserAsync(request.UserId, request.OperatorId, cancellationToken);

}
