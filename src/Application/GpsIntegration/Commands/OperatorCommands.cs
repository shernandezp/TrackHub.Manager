namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.Operators, Action = Actions.Edit)]

public readonly record struct SetOperatorEnabledCommand(Guid OperatorId, bool Enabled) : IRequest;
public class SetOperatorEnabledCommandHandler(IOperatorWriter writer) : IRequestHandler<SetOperatorEnabledCommand>
{
    public Task Handle(SetOperatorEnabledCommand request, CancellationToken cancellationToken)
        => writer.SetEnabledAsync(request.OperatorId, request.Enabled, cancellationToken);
}
