namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Edit)]
public readonly record struct EndDeviceTransporterAssignmentCommand(Guid AssignmentId, string? Reason) : IRequest;

public class EndDeviceTransporterAssignmentCommandHandler(ITransporterDeviceAssignmentWriter writer)
    : IRequestHandler<EndDeviceTransporterAssignmentCommand>
{
    public Task Handle(EndDeviceTransporterAssignmentCommand request, CancellationToken cancellationToken)
        => writer.EndAssignmentAsync(request.AssignmentId, request.Reason, cancellationToken);
}
