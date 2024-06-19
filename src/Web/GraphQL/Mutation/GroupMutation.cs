using TrackHub.Manager.Application.Groups.Commands.Create;
using TrackHub.Manager.Application.Groups.Commands.Delete;
using TrackHub.Manager.Application.Groups.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<GroupVm> CreateGroup([Service] ISender sender, CreateGroupCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateGroup([Service] ISender sender, Guid id, UpdateGroupCommand command)
    {
        if (id != command.Group.GroupId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteGroup([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteGroupCommand(id));
        return id;
    }
}
