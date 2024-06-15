namespace TrackHub.Manager.Application.Users.Commands.Create;

[Authorize(Resource = Resources.Users, Action = Actions.Write)]
public readonly record struct CreateUserCommand(UserDto User) : IRequest<UserVm>;

public class CreateUserCommandHandler(IUserWriter writer) : IRequestHandler<CreateUserCommand, UserVm>
{
    public async Task<UserVm> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        => await writer.CreateUserAsync(request.User, cancellationToken);
}
