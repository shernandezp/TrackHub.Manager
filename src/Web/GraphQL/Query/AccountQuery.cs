using TrackHub.Manager.Application.Accounts.Queries.Get;
using TrackHub.Manager.Application.Accounts.Queries.GetAll;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<AccountVm> GetAccount([Service] ISender sender, [AsParameters] GetAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AccountVm>> GetAccounts([Service] ISender sender, [AsParameters] GetAccountsQuery query)
        => await sender.Send(query);
}
