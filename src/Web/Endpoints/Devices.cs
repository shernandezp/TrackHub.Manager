using TrackHub.Manager.Application.Devices.Queries.Get;

namespace TrackHub.Manager.Web.Endpoints;

public class Devices : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .RequireAuthorization()
            .MapGet(GetDevice);
    }

    public async Task<DeviceVm> GetDevice(ISender sender, [AsParameters] GetDeviceQuery query)
        => await sender.Send(query);

}
