namespace TrackHub.Manager.Application.UserGroup.Commands.Create;

[Authorize(Resource = Resources.Groups, Action = Actions.Write)]
public readonly record struct CreateUserGroupCommand(UserGroupDto UserGroup) : IRequest<UserGroupVm>;

public class CreateUserGroupCommandHandler(IUserGroupWriter writer) : IRequestHandler<CreateUserGroupCommand, UserGroupVm>
{
    public async Task<UserGroupVm> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
        => await writer.CreateUserGroupAsync(request.UserGroup, cancellationToken);
}
