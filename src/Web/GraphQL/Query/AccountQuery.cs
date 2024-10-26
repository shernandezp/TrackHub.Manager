using TrackHub.Manager.Application.Accounts.Queries.Get;
using TrackHub.Manager.Application.Accounts.Queries.GetAll;
using TrackHub.Manager.Application.Accounts.Queries.GetSettings;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<AccountVm> GetAccount([Service] ISender sender, [AsParameters] GetAccountQuery query)
        => await sender.Send(query);

    public async Task<AccountVm> GetAccountByUser([Service] ISender sender)
        => await sender.Send(new GetAccountByUserQuery());

    public async Task<AccountSettingsVm> GetAccountSettings([Service] ISender sender, [AsParameters] GetAccountSettingsQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<AccountSettingsVm>> GetAccountsSettings([Service] ISender sender, [AsParameters] GetAccountsSettingsQuery query)
        => await sender.Send(query);

    public async Task<AccountSettingsVm> GetAccountSettingsByUser([Service] ISender sender)
        => await sender.Send(new GetAccountSettingsByUserQuery());

    public async Task<IReadOnlyCollection<AccountVm>> GetAccounts([Service] ISender sender)
        => await sender.Send(new GetAccountsQuery());

}
