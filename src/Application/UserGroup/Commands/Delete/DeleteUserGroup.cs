namespace TrackHub.Manager.Application.UserGroup.Commands.Delete;

[Authorize(Resource = Resources.Users, Action = Actions.Delete)]
public readonly record struct DeleteUserGroupCommand(Guid UserId, long GroupId) : IRequest;

public class DeleteUserGroupCommandHandler(IUserGroupWriter writer) : IRequestHandler<DeleteUserGroupCommand>
{

    public async Task Handle(DeleteUserGroupCommand request, CancellationToken cancellationToken)
        => await writer.DeleteUserGroupAsync(request.UserId, request.GroupId, cancellationToken);

}
