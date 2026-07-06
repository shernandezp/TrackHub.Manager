using TrackHub.Manager.Application.PointsOfInterest.Commands.Create;
using TrackHub.Manager.Application.PointsOfInterest.Commands.Delete;
using TrackHub.Manager.Application.PointsOfInterest.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<PointOfInterestVm> CreatePointOfInterest([Service] ISender sender, CreatePointOfInterestCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdatePointOfInterest([Service] ISender sender, Guid id, UpdatePointOfInterestCommand command)
    {
        if (id != command.Id) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeletePointOfInterest([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeletePointOfInterestCommand(id));
        return id;
    }
}
