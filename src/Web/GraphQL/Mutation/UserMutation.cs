using TrackHub.Manager.Application.Users.Commands.Create;
using TrackHub.Manager.Application.Users.Commands.Delete;
using TrackHub.Manager.Application.Users.Commands.Update;
using TrackHub.Manager.Application.Users.Commands.UpdateSettings;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<UserVm> CreateUser([Service] ISender sender, CreateUserCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateUser([Service] ISender sender, Guid id, UpdateUserCommand command)
    {
        if (id != command.User.UserId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<bool> UpdateUserSettings([Service] ISender sender, Guid id, UpdateUserSettingsCommand command)
    {
        if (id != command.UserSettings.UserId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteUser([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteUserCommand(id));
        return id;
    }
}
