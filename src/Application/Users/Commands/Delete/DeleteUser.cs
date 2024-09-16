namespace TrackHub.Manager.Application.Users.Commands.Delete;

[Authorize(Resource = Resources.Users, Action = Actions.Delete)]
public record DeleteUserCommand(Guid Id) : IRequest;

public class DeleteUserCommandHandler(IUserWriter writer, IUserSettingsWriter userSettingsWriter) : IRequestHandler<DeleteUserCommand>
{
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    { 
        await userSettingsWriter.DeleteUserSettingsAsync(request.Id, cancellationToken);
        await writer.DeleteUserAsync(request.Id, cancellationToken);
    }
}
