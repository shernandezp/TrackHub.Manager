using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.DeviceOperator.Queries.GetByOperator;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceOperatorByUserByOperatorQuery(Guid OperatorId) : IRequest<IReadOnlyCollection<DeviceOperatorVm>>;

public class GetDeviceOperatorByUserByOperatorQueryHandler(IDeviceOperatorReader reader, IUser user) : IRequestHandler<GetDeviceOperatorByUserByOperatorQuery, IReadOnlyCollection<DeviceOperatorVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<DeviceOperatorVm>> Handle(GetDeviceOperatorByUserByOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetDeviceOperatorsByUserAsync(UserId, request.OperatorId, cancellationToken);

}
