using TrackHub.Manager.Application.AlertEvents.Events;

namespace TrackHub.Manager.Application.AlertEvents.Commands;

[Authorize(Resource = Resources.Alerts, Action = Actions.Write)]
public readonly record struct RecordAlertEventCommand(AlertEventDto AlertEvent) : IRequest<AlertEventVm>;
public class RecordAlertEventCommandHandler(IAlertEventWriter writer, IPublisher publisher) : IRequestHandler<RecordAlertEventCommand, AlertEventVm>
{
    public async Task<AlertEventVm> Handle(RecordAlertEventCommand request, CancellationToken cancellationToken)
    {
        var alertEvent = await writer.RecordAlertEventAsync(request.AlertEvent, cancellationToken);
        // Rule evaluation is non-blocking: the event handler swallows its own failures.
        await publisher.Publish(new AlertEventRecorded.Notification(alertEvent), cancellationToken);
        return alertEvent;
    }
}

[Authorize(Resource = Resources.Alerts, Action = Actions.Edit)]
public readonly record struct AcknowledgeAlertEventCommand(Guid AlertEventId) : IRequest;
public class AcknowledgeAlertEventCommandHandler(IAlertEventWriter writer) : IRequestHandler<AcknowledgeAlertEventCommand>
{
    public async Task Handle(AcknowledgeAlertEventCommand request, CancellationToken cancellationToken) => await writer.AcknowledgeAlertEventAsync(request.AlertEventId, cancellationToken);
}

[Authorize(Resource = Resources.Alerts, Action = Actions.Edit)]
public readonly record struct ResolveAlertEventCommand(Guid AlertEventId) : IRequest;
public class ResolveAlertEventCommandHandler(IAlertEventWriter writer) : IRequestHandler<ResolveAlertEventCommand>
{
    public async Task Handle(ResolveAlertEventCommand request, CancellationToken cancellationToken) => await writer.ResolveAlertEventAsync(request.AlertEventId, cancellationToken);
}
