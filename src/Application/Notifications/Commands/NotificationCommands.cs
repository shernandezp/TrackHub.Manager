namespace TrackHub.Manager.Application.Notifications.Commands;

[Authorize(Resource = Resources.Notifications, Action = Actions.Write)]
public readonly record struct CreateNotificationRuleCommand(NotificationRuleDto NotificationRule) : IRequest<NotificationRuleVm>;
public class CreateNotificationRuleCommandHandler(INotificationWriter writer) : IRequestHandler<CreateNotificationRuleCommand, NotificationRuleVm>
{
    public async Task<NotificationRuleVm> Handle(CreateNotificationRuleCommand request, CancellationToken cancellationToken) => await writer.CreateNotificationRuleAsync(request.NotificationRule, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Edit)]
public readonly record struct UpdateNotificationRuleCommand(Guid NotificationRuleId, NotificationRuleDto NotificationRule) : IRequest;
public class UpdateNotificationRuleCommandHandler(INotificationWriter writer) : IRequestHandler<UpdateNotificationRuleCommand>
{
    public async Task Handle(UpdateNotificationRuleCommand request, CancellationToken cancellationToken) => await writer.UpdateNotificationRuleAsync(request.NotificationRuleId, request.NotificationRule, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Delete)]
public readonly record struct DisableNotificationRuleCommand(Guid NotificationRuleId) : IRequest;
public class DisableNotificationRuleCommandHandler(INotificationWriter writer) : IRequestHandler<DisableNotificationRuleCommand>
{
    public async Task Handle(DisableNotificationRuleCommand request, CancellationToken cancellationToken) => await writer.DisableNotificationRuleAsync(request.NotificationRuleId, cancellationToken);
}

[Authorize(Resource = Resources.Notifications, Action = Actions.Write)]
public readonly record struct CreateNotificationDeliveryCommand(NotificationDeliveryDto NotificationDelivery) : IRequest<NotificationDeliveryVm>;
public class CreateNotificationDeliveryCommandHandler(INotificationWriter writer) : IRequestHandler<CreateNotificationDeliveryCommand, NotificationDeliveryVm>
{
    public async Task<NotificationDeliveryVm> Handle(CreateNotificationDeliveryCommand request, CancellationToken cancellationToken) => await writer.CreateNotificationDeliveryAsync(request.NotificationDelivery, cancellationToken);
}
