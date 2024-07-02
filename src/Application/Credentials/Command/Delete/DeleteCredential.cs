namespace TrackHub.Manager.Application.Credentials.Command.Delete;

[Authorize(Resource = Resources.Credentials, Action = Actions.Delete)]
public record DeleteCredentialCommand(Guid Id) : IRequest;

public class DeleteCredentialCommandHandler(ICredentialWriter writer) : IRequestHandler<DeleteCredentialCommand>
{
    public async Task Handle(DeleteCredentialCommand request, CancellationToken cancellationToken)
        => await writer.DeleteCredentialAsync(request.Id, cancellationToken);
}
