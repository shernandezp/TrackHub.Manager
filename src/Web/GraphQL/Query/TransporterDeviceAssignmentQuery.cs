using TrackHub.Manager.Application.GpsIntegration.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetTransporterDeviceAssignmentsByTransporter([Service] ISender sender, [AsParameters] GetTransporterDeviceAssignmentsByTransporterQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetTransporterDeviceAssignmentsByDevice([Service] ISender sender, [AsParameters] GetTransporterDeviceAssignmentsByDeviceQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetTransporterDeviceAssignmentsByAccount([Service] ISender sender, [AsParameters] GetTransporterDeviceAssignmentsByAccountQuery query)
        => await sender.Send(query);
}
