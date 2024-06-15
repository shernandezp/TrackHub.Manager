namespace TrackHub.Manager.Application.Transporters.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public record DeleteTransporterCommand(Guid Id) : IRequest;

public class DeleteTransporterCommandHandler(ITransporterWriter writer) : IRequestHandler<DeleteTransporterCommand>
{
    public async Task Handle(DeleteTransporterCommand request, CancellationToken cancellationToken)
        => await writer.DeleteTransporterAsync(request.Id, cancellationToken);
}
