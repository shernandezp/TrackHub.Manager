using TrackHub.Manager.Application.Transporters.Queries.Get;
using TrackHub.Manager.Application.Transporters.Queries.GetByAccount;
using TrackHub.Manager.Application.Transporters.Queries.GetByGroup;
using TrackHub.Manager.Application.Transporters.Queries.GetByUser;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<TransporterVm> GetTransporter([Service] ISender sender, [AsParameters] GetTransporterQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByAccount([Service] ISender sender)
        => await sender.Send(new GetTransportersByAccountQuery());

    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByGroup([Service] ISender sender, [AsParameters] GetTransporterByGroupQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByUser([Service] ISender sender)
        => await sender.Send(new GetTransporterByUserQuery());

}
