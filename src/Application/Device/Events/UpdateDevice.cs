namespace TrackHub.Manager.Application.Device.Events;

public sealed class UpdateDevice
{
    // Represents a notification for when a device needs to be updated
    public readonly record struct Notification(UpdateDeviceDto Device) : INotification
    {
        // Handles the UpdateDevice notification
        public class EventHandler(IDeviceWriter deviceWriter) : INotificationHandler<Notification>
        {
            // Handles the notification by calling the UpdateDeviceAsync method on the device writer
            public async Task Handle(Notification notification, CancellationToken cancellationToken)
                => await deviceWriter.UpdateDeviceAsync(notification.Device, cancellationToken);
        }
    }
}
