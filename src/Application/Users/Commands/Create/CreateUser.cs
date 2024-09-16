using TrackHub.Manager.Application.Users.Events;

namespace TrackHub.Manager.Application.Users.Commands.Create;

[Authorize(Resource = Resources.Users, Action = Actions.Write)]
public readonly record struct CreateUserCommand(UserDto User) : IRequest<UserVm>;

public class CreateUserCommandHandler(IUserWriter writer, IPublisher publisher) : IRequestHandler<CreateUserCommand, UserVm>
{
    public async Task<UserVm> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    { 
        var user = await writer.CreateUserAsync(request.User, cancellationToken);
        await publisher.Publish(new UserCreated.Notification(user.UserId), cancellationToken);
        return user;
    }
}
