using TrackHub.Manager.Application.Drivers.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DriverVm> CreateDriver([Service] ISender sender, CreateDriverCommand command) => await sender.Send(command);
    public async Task<bool> UpdateDriver([Service] ISender sender, UpdateDriverCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DeactivateDriver([Service] ISender sender, DeactivateDriverCommand command) { await sender.Send(command); return true; }

    public async Task<DriverQualificationVm> CreateDriverQualification([Service] ISender sender, CreateDriverQualificationCommand command) => await sender.Send(command);
    public async Task<bool> UpdateDriverQualification([Service] ISender sender, UpdateDriverQualificationCommand command) { await sender.Send(command); return true; }
    // Delete mutations return the deleted entity's identifier, not a boolean (rules.md "Naming").
    public async Task<Guid> DeleteDriverQualification([Service] ISender sender, DeleteDriverQualificationCommand command) { await sender.Send(command); return command.DriverQualificationId; }

    public async Task<DriverTransporterAssignmentVm> AssignDriverToTransporter([Service] ISender sender, AssignDriverToTransporterCommand command) => await sender.Send(command);
    public async Task<bool> EndDriverAssignment([Service] ISender sender, EndDriverAssignmentCommand command) { await sender.Send(command); return true; }
}
