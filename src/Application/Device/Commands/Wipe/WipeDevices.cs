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

using TrackHub.Manager.Application.Device.Events;

namespace TrackHub.Manager.Application.Device.Commands.Wipe;

[Authorize(Resource = Resources.Devices, Action = Actions.Delete)]
public readonly record struct WipeDevicesCommand(Guid OperatorId) : IRequest;

public class WipeDevicesCommandHandler(
        IPublisher publisher,
        IDeviceWriter writer,
        IDeviceReader reader) : IRequestHandler<WipeDevicesCommand>
{
    // This method handles the execution of the WipeDevicesCommand.
    // It deletes all devices associated with the specified operator.
    public async Task Handle(WipeDevicesCommand request, CancellationToken cancellationToken)
    {
        var devices = await reader.GetDevicesByOperatorAsync(request.OperatorId, cancellationToken);
        foreach (var device in devices)
        {
            await writer.DeleteDeviceAsync(device.DeviceId, cancellationToken);
            if (device.TransporterId != null)
            {
                await publisher.Publish(new DeviceDeleted.Notification(device.TransporterId.Value, device.DeviceId), cancellationToken);
            }
        }
    }
}
