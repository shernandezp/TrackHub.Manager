namespace TrackHub.Manager.Application.AuditEvents.Commands;

[Authorize(Resource = Resources.Audit, Action = Actions.Write)]
public readonly record struct CreateAuditEventCommand(AuditEventDto AuditEvent) : IRequest<AuditEventVm>;
public class CreateAuditEventCommandHandler(IAuditEventWriter writer) : IRequestHandler<CreateAuditEventCommand, AuditEventVm>
{
    public async Task<AuditEventVm> Handle(CreateAuditEventCommand request, CancellationToken cancellationToken) => await writer.CreateAuditEventAsync(request.AuditEvent, cancellationToken);
}
