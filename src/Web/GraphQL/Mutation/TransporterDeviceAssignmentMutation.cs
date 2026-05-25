using TrackHub.Manager.Application.GpsIntegration.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<TransporterDeviceAssignmentVm> AssignDeviceToTransporter([Service] ISender sender, AssignDeviceToTransporterCommand command)
        => await sender.Send(command);

    public async Task<bool> EndDeviceTransporterAssignment([Service] ISender sender, EndDeviceTransporterAssignmentCommand command)
    { await sender.Send(command); return true; }
}
