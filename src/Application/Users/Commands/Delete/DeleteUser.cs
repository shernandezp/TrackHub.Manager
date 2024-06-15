namespace TrackHub.Manager.Application.Users.Commands.Delete;

[Authorize(Resource = Resources.Users, Action = Actions.Delete)]
public record DeleteUserCommand(Guid Id) : IRequest;

public class DeleteUserCommandHandler(IUserWriter writer) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        => await writer.DeleteUserAsync(request.Id, cancellationToken);
}
