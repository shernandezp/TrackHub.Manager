using TrackHub.Manager.Application.Drivers.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DriverVm> CreateDriver([Service] ISender sender, CreateDriverCommand command) => await sender.Send(command);
    public async Task<bool> UpdateDriver([Service] ISender sender, UpdateDriverCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DeactivateDriver([Service] ISender sender, DeactivateDriverCommand command) { await sender.Send(command); return true; }
}
