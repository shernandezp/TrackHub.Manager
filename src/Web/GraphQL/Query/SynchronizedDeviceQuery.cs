using TrackHub.Manager.Application.GpsIntegration.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<DeviceVm>> GetSynchronizedDevices([Service] ISender sender, [AsParameters] GetSynchronizedDevicesQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceVm>> GetUnassignedSynchronizedDevices([Service] ISender sender, [AsParameters] GetUnassignedSynchronizedDevicesQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<DeviceTransporterVm>> GetAssignedDeviceTransportersByOperator(
        [Service] ISender sender,
        [AsParameters] GetAssignedDeviceTransportersByOperatorQuery query)
        => await sender.Send(query);
}
