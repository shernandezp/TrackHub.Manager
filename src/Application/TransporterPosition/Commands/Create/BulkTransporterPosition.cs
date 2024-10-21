namespace TrackHub.Manager.Application.TransporterPosition.Commands.Create;

[Authorize(Resource = Resources.Positions, Action = Actions.Write)]
public readonly record struct BulkTransporterPositionCommand(IEnumerable<TransporterPositionDto> Positions) : IRequest;

public class CreateTransporterCommandHandler(ITransporterPositionWriter writer) : IRequestHandler<BulkTransporterPositionCommand>
{
    public async Task Handle(BulkTransporterPositionCommand request, CancellationToken cancellationToken)
    {
        await writer.BulkTransporterPositionAsync(request.Positions, cancellationToken);
    }
}
