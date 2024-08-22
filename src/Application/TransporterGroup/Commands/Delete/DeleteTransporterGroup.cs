namespace TrackHub.Manager.Application.TransporterGroup.Commands.Delete;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct DeleteTransporterGroupCommand(Guid TransporterId, long GroupId) : IRequest;

public class DeleteTransporterGroupCommandHandler(ITransporterGroupWriter writer) : IRequestHandler<DeleteTransporterGroupCommand>
{

    public async Task Handle(DeleteTransporterGroupCommand request, CancellationToken cancellationToken)
        => await writer.DeleteTransporterGroupAsync(request.TransporterId, request.GroupId, cancellationToken);

}
