using TrackHub.Manager.Application.UserGroup.Commands.Create;
using TrackHub.Manager.Application.UserGroup.Commands.Delete;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<UserGroupVm> CreateUserGroup([Service] ISender sender, CreateUserGroupCommand command)
        => await sender.Send(command);

    public async Task<Guid> DeleteUserGroup([Service] ISender sender, Guid UserId, long GroupId)
    {
        await sender.Send(new DeleteUserGroupCommand(UserId, GroupId));
        return UserId;
    }
}
