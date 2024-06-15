namespace TrackHub.Manager.Application.Operators.Commands.Delete;

[Authorize(Resource = Resources.Accounts, Action = Actions.Delete)]
public record DeleteOperatorCommand(Guid Id) : IRequest;

public class DeleteOperatorCommandHandler(IOperatorWriter writer) : IRequestHandler<DeleteOperatorCommand>
{
    public async Task Handle(DeleteOperatorCommand request, CancellationToken cancellationToken)
        => await writer.DeleteOperatorAsync(request.Id, cancellationToken);
}
