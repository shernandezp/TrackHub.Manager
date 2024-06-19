namespace TrackHub.Manager.Application.Accounts.Queries.Get;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountQuery(Guid Id) : IRequest<AccountVm>;

public class GetAccountQueryHandler(IAccountReader reader) : IRequestHandler<GetAccountQuery, AccountVm>
{
    public async Task<AccountVm> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountAsync(request.Id, cancellationToken);

}
