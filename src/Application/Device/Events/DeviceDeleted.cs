namespace TrackHub.Manager.Application.Device.Events;

public sealed class DeviceDeleted
{
    // Represents a notification that a device was deleted
    public readonly record struct Notification(Guid TransporterId, Guid DeviceId) : INotification
    {
        // Initializes a new instance of the Notification class
        public class EventHandler(
            ITransporterWriter transporterWriter,
            ITransporterGroupWriter transporterGroupWriter,
            IDeviceReader deviceReader) : INotificationHandler<Notification>
        {
            // Handles the notification.
            // It checks if the transporter is associated with a group and deletes the association.
            // It also deletes the transporter if there are no devices associated with it.
            public async Task Handle(Notification notification, CancellationToken cancellationToken)
            { 
                var exists = await deviceReader.ExistDeviceAsync(notification.TransporterId, notification.DeviceId, cancellationToken);
                if (!exists) 
                {
                    await transporterGroupWriter.DeleteTransporterGroupsAsync(notification.TransporterId, cancellationToken);
                    await transporterWriter.DeleteTransporterAsync(notification.TransporterId, cancellationToken);
                }
            }
        }
    }
}
