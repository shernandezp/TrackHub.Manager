using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Transporters.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetTransportersByAccountQuery() : IRequest<IReadOnlyCollection<TransporterVm>>;

public class GetTransportersByAccountQueryHandler(ITransporterReader reader, IAccountReader accountReader, IUser user) : IRequestHandler<GetTransportersByAccountQuery, IReadOnlyCollection<TransporterVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    public async Task<IReadOnlyCollection<TransporterVm>> Handle(GetTransportersByAccountQuery request, CancellationToken cancellationToken)
    {
        var account = await accountReader.GetAccountByUserIdAsync(UserId, cancellationToken);
        return await reader.GetTransportersByAccountAsync(account.AccountId, cancellationToken);
    }

}
