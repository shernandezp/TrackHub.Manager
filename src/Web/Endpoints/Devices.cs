using TrackHub.Manager.Application.Transporters.Queries.Get;

namespace TrackHub.Manager.Web.Endpoints;

public class Devices : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDevice);
    }

    public async Task<TransporterVm> GetDevice(ISender sender, [AsParameters] GetTransporterQuery query)
        => await sender.Send(query);

}
