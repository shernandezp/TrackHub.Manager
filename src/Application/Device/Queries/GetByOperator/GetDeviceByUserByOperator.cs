using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Device.Queries.GetByOperator;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByUserByOperatorQuery(Guid OperatorId) : IRequest<IReadOnlyCollection<DeviceTransporterVm>>;

public class GetDeviceByUserByOperatorQueryHandler(IDeviceReader reader, IUser user) : IRequestHandler<GetDeviceByUserByOperatorQuery, IReadOnlyCollection<DeviceTransporterVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<DeviceTransporterVm>> Handle(GetDeviceByUserByOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByUserAsync(UserId, request.OperatorId, cancellationToken);

}
