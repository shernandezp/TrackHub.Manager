using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Devices.Queries.GetByUser;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDeviceByUserQuery() : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesQueryHandler(IDeviceReader reader, IUser user) : IRequestHandler<GetDeviceByUserQuery, IReadOnlyCollection<DeviceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDeviceByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetDevicesByUserAsync(UserId, cancellationToken);

}
