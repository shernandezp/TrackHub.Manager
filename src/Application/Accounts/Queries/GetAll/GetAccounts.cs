namespace TrackHub.Manager.Application.Accounts.Queries.GetAll;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountQuery() : IRequest<IReadOnlyCollection<AccountVm>>;

public class GetAccountsQueryHandler(IAccountReader reader) : IRequestHandler<GetAccountQuery, IReadOnlyCollection<AccountVm>>
{
    public async Task<IReadOnlyCollection<AccountVm>> Handle(GetAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountsAsync(cancellationToken);

}
