namespace TrackHub.Manager.Application.Accounts.Queries.GetAll;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountsQuery() : IRequest<IReadOnlyCollection<AccountVm>>;

public class GetAccountsQueryHandler(IAccountReader reader) : IRequestHandler<GetAccountsQuery, IReadOnlyCollection<AccountVm>>
{
    public async Task<IReadOnlyCollection<AccountVm>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountsAsync(cancellationToken);

}
