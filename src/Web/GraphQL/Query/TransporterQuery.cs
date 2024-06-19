using TrackHub.Manager.Application.Transporters.Queries.Get;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<TransporterVm> GetTransporter([Service] ISender sender, [AsParameters] GetTransporterQuery query)
        => await sender.Send(query);

}
