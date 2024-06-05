namespace TrackHub.Manager.Application.Users.Commands.Delete;
public record DeleteUserCommand(Guid Id) : IRequest;

public class DeleteUserCommandHandler(IUserWriter writer) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        => await writer.DeleteUserAsync(request.Id, cancellationToken);
}
