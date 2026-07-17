using TrackHub.Manager.Domain.Constants;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Notifications;

/// <summary>
/// In-app channel: the delivery row itself is the notification (surfaced by getMyNotifications);
/// sending is marking it Sent (spec 05 §7.4).
/// </summary>
public sealed class InAppNotificationProvider : INotificationChannelProvider
{
    public string Channel => NotificationChannels.InApp;

    public Task<NotificationSendResult> SendAsync(NotificationMessage message, CancellationToken cancellationToken)
        => Task.FromResult(new NotificationSendResult(true, null, null));
}
