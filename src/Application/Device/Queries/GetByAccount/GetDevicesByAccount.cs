using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Device.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDevicesByAccountQuery() : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesByAccountQueryHandler(IDeviceReader reader, IAccountReader accountReader, IUser user) : IRequestHandler<GetDevicesByAccountQuery, IReadOnlyCollection<DeviceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDevicesByAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await accountReader.GetAccountByUserIdAsync(UserId, cancellationToken);
        return await reader.GetDevicesByAccountAsync(account.AccountId, cancellationToken);
    }

}
