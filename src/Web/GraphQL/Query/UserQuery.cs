using TrackHub.Manager.Application.Users.Queries.Get;
using TrackHub.Manager.Application.Users.Queries.GetByAccount;
using TrackHub.Manager.Application.Users.Queries.GetByGroup;
using TrackHub.Manager.Application.Users.Queries.GetSettings;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<UserVm> GetUser([Service] ISender sender, [AsParameters] GetUserQuery query)
        => await sender.Send(query);

    public async Task<UserSettingsVm> GetUserSettings([Service] ISender sender)
        => await sender.Send(new GetUserSettingsQuery());

    public async Task<IReadOnlyCollection<UserVm>> GetUsersByAccount([Service] ISender sender, [AsParameters] GetUsersByAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<UserVm>> GetUsersByGroup([Service] ISender sender, [AsParameters] GetUsersByGroupQuery query)
        => await sender.Send(query);
}
