namespace TrackHub.Manager.Application.Accounts.Events;

public sealed class AccountCreated
{
    // Represents a notification for when an account is created
    public readonly record struct Notification(Guid AccountId) : INotification
    {
        // Handles the AccountCreated notification
        public class EventHandler(IAccountSettingsWriter writer) : INotificationHandler<Notification>
        {
            // Handles the notification by calling the CreateAccountSettingsAsync method
            public async Task Handle(Notification notification, CancellationToken cancellationToken)
                => await writer.CreateAccountSettingsAsync(notification.AccountId, cancellationToken);
        }
    }
}
