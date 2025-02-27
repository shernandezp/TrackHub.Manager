// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

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
            ITransporterPositionWriter transporterPositionWriter,
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
                    await transporterPositionWriter.DeleteTransporterPositionAsync(notification.TransporterId, cancellationToken);
                    await transporterWriter.DeleteTransporterAsync(notification.TransporterId, cancellationToken);
                }
            }
        }
    }
}
