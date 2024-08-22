using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Transporters.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetTransportersByCurrentAccountQuery() : IRequest<IReadOnlyCollection<TransporterVm>>;

public class GetTransportersByCurrentAccountQueryHandler(ITransporterReader reader, IAccountReader accountReader, IUser user) : IRequestHandler<GetTransportersByCurrentAccountQuery, IReadOnlyCollection<TransporterVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<IReadOnlyCollection<TransporterVm>> Handle(GetTransportersByCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await accountReader.GetAccountByUserIdAsync(UserId, cancellationToken);
        return await reader.GetTransportersByAccountAsync(account.AccountId, cancellationToken);
    }

}
