using TrackHub.Manager.Application.Notifications.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<NotificationRuleVm> CreateNotificationRule([Service] ISender sender, CreateNotificationRuleCommand command) => await sender.Send(command);
    public async Task<bool> UpdateNotificationRule([Service] ISender sender, UpdateNotificationRuleCommand command) { await sender.Send(command); return true; }
    public async Task<bool> DisableNotificationRule([Service] ISender sender, DisableNotificationRuleCommand command) { await sender.Send(command); return true; }
    public async Task<NotificationDeliveryVm> CreateNotificationDelivery([Service] ISender sender, CreateNotificationDeliveryCommand command) => await sender.Send(command);
}
