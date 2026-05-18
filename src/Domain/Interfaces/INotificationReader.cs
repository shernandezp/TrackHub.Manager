namespace TrackHub.Manager.Domain.Interfaces;

public interface INotificationReader
{
    Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRulesAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken);
}
