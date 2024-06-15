namespace TrackHub.Manager.Application.Groups.Commands.Delete;

[Authorize(Resource = Resources.Groups, Action = Actions.Delete)]
public record DeleteGroupCommand(Guid Id) : IRequest;

public class DeleteGroupCommandHandler(IGroupWriter writer) : IRequestHandler<DeleteGroupCommand>
{
    public async Task Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        => await writer.DeleteGroupAsync(request.Id, cancellationToken);
}
