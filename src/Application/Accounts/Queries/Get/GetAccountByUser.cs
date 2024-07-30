using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Accounts.Queries.Get;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountByUserQuery() : IRequest<AccountVm>;

public class GetAccountByUserQueryHandler(IAccountReader reader, IUser user) : IRequestHandler<GetAccountByUserQuery, AccountVm>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);
    // This method handles the GetAccountQuery and returns an AccountVm
    public async Task<AccountVm> Handle(GetAccountByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountByUserIdAsync(UserId, cancellationToken);

}
