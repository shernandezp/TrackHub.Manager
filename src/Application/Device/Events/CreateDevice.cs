namespace TrackHub.Manager.Application.Device.Events;

public sealed class CreateDevice
{
    // Represents a notification for when a device needs to be created
    public readonly record struct Notification(DeviceDto Device) : INotification
    {
        // Handles the CreateDevice notification
        public class EventHandler(IDeviceWriter deviceWriter) : INotificationHandler<Notification>
        {
            // Handles the notification by calling the CreateDeviceAsync method on the device writer
            public async Task Handle(Notification notification, CancellationToken cancellationToken)
                => await deviceWriter.CreateDeviceAsync(notification.Device, cancellationToken);
        }
    }
}
