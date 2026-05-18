using TrackHub.Manager.Application.Notifications.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRules([Service] ISender sender, [AsParameters] GetNotificationRulesQuery query) => await sender.Send(query);
}
