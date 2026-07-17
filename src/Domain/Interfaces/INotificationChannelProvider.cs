namespace TrackHub.Manager.Domain.Interfaces;

/// <summary>
/// Channel delivery provider contract (spec 05 §7.4, §9). Implementations: InApp, Email (MailKit),
/// Webhook (HMAC-signed), WhatsApp (Meta Cloud API). Push is contract-only in this slice — the FCM
/// implementation, unmasked-token query, and credentials ship with spec 10.
/// </summary>
public interface INotificationChannelProvider
{
    /// <summary>One of <c>NotificationChannels</c>.</summary>
    string Channel { get; }

    Task<NotificationSendResult> SendAsync(NotificationMessage message, CancellationToken cancellationToken);
}
