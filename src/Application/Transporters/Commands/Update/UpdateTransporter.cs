namespace TrackHub.Manager.Application.Transporters.Commands.Update;

[Authorize(Resource = Resources.Devices, Action = Actions.Edit)]
public readonly record struct UpdateTransporterCommand(UpdateTransporterDto Transporter) : IRequest;

public class UpdateTransporterCommandHandler(ITransporterWriter writer) : IRequestHandler<UpdateTransporterCommand>
{
    public async Task Handle(UpdateTransporterCommand request, CancellationToken cancellationToken)
        => await writer.UpdateTransporterAsync(request.Transporter, cancellationToken);
}
