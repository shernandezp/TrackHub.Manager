using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Devices.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetDevicesByCurrentAccountQuery() : IRequest<IReadOnlyCollection<DeviceVm>>;

public class GetDevicesByCurrentAccountQueryHandler(IDeviceReader reader, IAccountReader accountReader, IUser user) : IRequestHandler<GetDevicesByCurrentAccountQuery, IReadOnlyCollection<DeviceVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<IReadOnlyCollection<DeviceVm>> Handle(GetDevicesByCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await accountReader.GetAccountByUserIdAsync(UserId, cancellationToken);
        return await reader.GetDevicesByAccountAsync(account.AccountId, cancellationToken);
    }

}
