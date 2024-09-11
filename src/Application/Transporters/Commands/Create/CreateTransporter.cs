namespace TrackHub.Manager.Application.Transporters.Commands.Create;

[Authorize(Resource = Resources.Transporters, Action = Actions.Write)]
public readonly record struct CreateTransporterCommand(TransporterDto Transporter) : IRequest<TransporterVm>;

public class CreateTransporterCommandHandler(ITransporterWriter writer) : IRequestHandler<CreateTransporterCommand, TransporterVm>
{
    public async Task<TransporterVm> Handle(CreateTransporterCommand request, CancellationToken cancellationToken)
        => await writer.CreateTransporterAsync(request.Transporter, cancellationToken);
}
