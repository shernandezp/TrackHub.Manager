using TrackHub.Manager.Application.Notifications.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRules([Service] ISender sender, [AsParameters] GetNotificationRulesQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<NotificationDeliveryVm>> GetNotificationDeliveries([Service] ISender sender, [AsParameters] GetNotificationDeliveriesQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<AlertSubscriptionVm>> GetAlertSubscriptions([Service] ISender sender, [AsParameters] GetAlertSubscriptionsQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<NotificationTemplateVm>> GetNotificationTemplates([Service] ISender sender, [AsParameters] GetNotificationTemplatesQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<MyNotificationVm>> GetMyNotifications([Service] ISender sender, [AsParameters] GetMyNotificationsQuery query) => await sender.Send(query);
    public async Task<IReadOnlyCollection<DeliveryHealthVm>> GetDeliveryHealth([Service] ISender sender, [AsParameters] GetDeliveryHealthQuery query) => await sender.Send(query);
}
