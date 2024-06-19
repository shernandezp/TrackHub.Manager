using TrackHub.Manager.Application.Groups.Queries.Get;
using TrackHub.Manager.Application.Groups.Queries.GetByAccount;
using TrackHub.Manager.Application.Groups.Queries.GetByUser;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<GroupVm> GetGroup([Service] ISender sender, [AsParameters] GetGroupQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByAccount([Service] ISender sender, [AsParameters] GetGroupByAccountQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByUser([Service] ISender sender, [AsParameters] GetGroupByUserQuery query)
        => await sender.Send(query);
}
