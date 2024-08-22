using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Device.Queries.GetByOperator;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByUserByOperatorQuery(Guid OperatorId) : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDeviceByUserByOperatorQueryHandler(IDeviceReader reader, IUser user) : IRequestHandler<GetDeviceByUserByOperatorQuery, IReadOnlyCollection<DeviceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByUserByOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByUserAsync(UserId, request.OperatorId, cancellationToken);

}
