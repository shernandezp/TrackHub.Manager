namespace TrackHub.Manager.Application.Users.Commands.Update;

[Authorize(Resource = Resources.AccountScreen, Action = Actions.Edit)]
public readonly record struct UpdateUserCommand(UserDto User) : IRequest;

public class UpdateUserCommandHandler(IUserWriter writer) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        => await writer.UpdateUserAsync(request.User, cancellationToken);
}
