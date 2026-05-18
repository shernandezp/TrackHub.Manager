namespace TrackHub.Manager.Application.Notifications.Queries;

[Authorize(Resource = Resources.Notifications, Action = Actions.Read)]
public readonly record struct GetNotificationRulesQuery(Guid AccountId, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<NotificationRuleVm>>;
public class GetNotificationRulesQueryHandler(INotificationReader reader) : IRequestHandler<GetNotificationRulesQuery, IReadOnlyCollection<NotificationRuleVm>>
{
    public async Task<IReadOnlyCollection<NotificationRuleVm>> Handle(GetNotificationRulesQuery request, CancellationToken cancellationToken) => await reader.GetNotificationRulesAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}
