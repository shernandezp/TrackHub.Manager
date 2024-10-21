namespace TrackHub.Manager.Application.Transporters.Commands.Delete;

[Authorize(Resource = Resources.Transporters, Action = Actions.Delete)]
public record DeleteTransporterCommand(Guid Id) : IRequest;

public class DeleteTransporterCommandHandler(ITransporterWriter writer, ITransporterPositionWriter transporterPositionWriter) : IRequestHandler<DeleteTransporterCommand>
{
    public async Task Handle(DeleteTransporterCommand request, CancellationToken cancellationToken)
    { 
        await transporterPositionWriter.DeleteTransporterPositionAsync(request.Id, cancellationToken);
        await writer.DeleteTransporterAsync(request.Id, cancellationToken);
    }
}
