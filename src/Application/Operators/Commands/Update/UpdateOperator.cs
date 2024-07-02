namespace TrackHub.Manager.Application.Operators.Commands.Update;

[Authorize(Resource = Resources.Operators, Action = Actions.Edit)]
public readonly record struct UpdateOperatorCommand(UpdateOperatorDto Operator) : IRequest;

public class UpdateOperatorCommandHandler(IOperatorWriter writer) : IRequestHandler<UpdateOperatorCommand>
{
    public async Task Handle(UpdateOperatorCommand request, CancellationToken cancellationToken)
        => await writer.UpdateOperatorAsync(request.Operator, cancellationToken);
}
