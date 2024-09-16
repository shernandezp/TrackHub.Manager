namespace TrackHub.Manager.Application.Users.Events;

public sealed class UserCreated
{
    // Represents a notification for when a user is created
    public readonly record struct Notification(Guid UserId) : INotification
    {
        // Handles the UserCreated notification
        public class EventHandler(IUserSettingsWriter writer) : INotificationHandler<Notification>
        {
            // Handles the notification by calling the CreateUserSettingsAsync method
            public async Task Handle(Notification notification, CancellationToken cancellationToken)
                => await writer.CreateUserSettingsAsync(notification.UserId, cancellationToken);
        }
    }
}
