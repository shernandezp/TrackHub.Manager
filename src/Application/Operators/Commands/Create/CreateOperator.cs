namespace TrackHub.Manager.Application.Operators.Commands.Create;

[Authorize(Resource = Resources.Accounts, Action = Actions.Write)]
public readonly record struct CreateOperatorCommand(OperatorDto Operator) : IRequest<OperatorVm>;

public class CreateOperatorCommandHandler(IOperatorWriter writer) : IRequestHandler<CreateOperatorCommand, OperatorVm>
{
    public async Task<OperatorVm> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
        => await writer.CreateOperatorAsync(request.Operator, cancellationToken);
}
