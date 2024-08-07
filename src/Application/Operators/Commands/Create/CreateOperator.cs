using TrackHub.Manager.Application.Operators.Events;

namespace TrackHub.Manager.Application.Operators.Commands.Create;

[Authorize(Resource = Resources.Operators, Action = Actions.Write)]
public readonly record struct CreateOperatorCommand(OperatorDto Operator) : IRequest<OperatorVm>;

public class CreateOperatorCommandHandler(IOperatorWriter writer, IPublisher publisher) : IRequestHandler<CreateOperatorCommand, OperatorVm>
{
    public async Task<OperatorVm> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
    {
        // Publish a notification for the newly created operator - Create the credential
        await publisher.Publish(new OperatorCreated.Notification(request.Operator.Credential), cancellationToken);

        // Create the operator using the writer
        return await writer.CreateOperatorAsync(request.Operator, cancellationToken);
    }
}
