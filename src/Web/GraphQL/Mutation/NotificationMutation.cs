using TrackHub.Manager.Application.Notifications.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<NotificationRuleVm> CreateNotificationRule([Service] ISender sender, CreateNotificationRuleCommand command) => await sender.Send(command);
    public async Task<bool> UpdateNotificationRule([Service] ISender sender, UpdateNotificationRuleCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DisableNotificationRule([Service] ISender sender, DisableNotificationRuleCommand command) { await sender.Send(command); return true; }
    public async Task<NotificationDeliveryVm> CreateNotificationDelivery([Service] ISender sender, CreateNotificationDeliveryCommand command) => await sender.Send(command);

    public async Task<AlertSubscriptionVm> CreateAlertSubscription([Service] ISender sender, CreateAlertSubscriptionCommand command) => await sender.Send(command);
    public async Task<bool> UpdateAlertSubscription([Service] ISender sender, UpdateAlertSubscriptionCommand command) { await sender.Send(command); return true; }
    public async Task<Guid> DeleteAlertSubscription([Service] ISender sender, DeleteAlertSubscriptionCommand command) => await sender.Send(command);

    public async Task<NotificationTemplateVm> CreateNotificationTemplate([Service] ISender sender, CreateNotificationTemplateCommand command) => await sender.Send(command);
    public async Task<bool> UpdateNotificationTemplate([Service] ISender sender, UpdateNotificationTemplateCommand command) { await sender.Send(command); return true; }
    public async Task<Guid> DeleteNotificationTemplate([Service] ISender sender, DeleteNotificationTemplateCommand command) => await sender.Send(command);

    public async Task<bool> RetryNotificationDelivery([Service] ISender sender, RetryNotificationDeliveryCommand command) { await sender.Send(command); return true; }
    public async Task<bool> MarkNotificationRead([Service] ISender sender, MarkNotificationReadCommand command) { await sender.Send(command); return true; }
    public async Task<NotificationDeliveryVm> SendTestNotification([Service] ISender sender, SendTestNotificationCommand command) => await sender.Send(command);
}
