namespace TrackHub.Manager.Application.TransporterGroup.Commands.Create;

[Authorize(Resource = Resources.Groups, Action = Actions.Write)]
public readonly record struct CreateTransporterGroupCommand(TransporterGroupDto TransporterGroup) : IRequest<TransporterGroupVm>;

public class CreateTransporterGroupCommandHandler(ITransporterGroupWriter writer) : IRequestHandler<CreateTransporterGroupCommand, TransporterGroupVm>
{
    public async Task<TransporterGroupVm> Handle(CreateTransporterGroupCommand request, CancellationToken cancellationToken)
        => await writer.CreateTransporterGroupAsync(request.TransporterGroup, cancellationToken);
}
