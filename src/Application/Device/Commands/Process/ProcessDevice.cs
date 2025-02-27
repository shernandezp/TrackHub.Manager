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

namespace TrackHub.Manager.Application.Device.Commands.Process;

[Authorize(Resource = Resources.Devices, Action = Actions.Write)]
public readonly record struct ProcessDeviceCommand(ProcessDeviceDto ProcessDevice, Guid OperatorId) : IRequest<bool>;

// ProcessDeviceCommandHandler class for handling the ProcessDeviceCommand
public class ProcessDeviceCommandHandler(
    IPublisher publisher,
    IDeviceReader deviceReader,
    ITransporterWriter transporterWriter,
    ITransporterReader transporterReader) : IRequestHandler<ProcessDeviceCommand, bool>
{

    /// <summary>
    /// Handles the ProcessDeviceCommand
    /// </summary>
    /// <param name="request">The ProcessDeviceCommand object</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains a boolean value indicating the success of the operation</returns>
    public async Task<bool> Handle(ProcessDeviceCommand request, CancellationToken cancellationToken)
    {
        var transporter = await GetTransporter(request.ProcessDevice, cancellationToken);
        var device = await deviceReader.GetDeviceAsync(request.ProcessDevice.Serial, request.OperatorId, cancellationToken);
        if (device == default)
        {
            var deviceDto = new DeviceDto(
                request.ProcessDevice.Name,
                request.ProcessDevice.Identifier,
                request.ProcessDevice.Serial,
                request.ProcessDevice.DeviceTypeId,
                request.ProcessDevice.Description,
                transporter.TransporterId,
                request.OperatorId);
            await publisher.Publish(new CreateDevice.Notification(deviceDto), cancellationToken);
        }
        else
        {
            var deviceDto = new UpdateDeviceDto(
                device.DeviceId,
                request.ProcessDevice.Name,
                request.ProcessDevice.Identifier,
                request.ProcessDevice.DeviceTypeId,
                request.ProcessDevice.Description,
                transporter.TransporterId);
            await publisher.Publish(new UpdateDevice.Notification(deviceDto), cancellationToken);
        }
        return true;
    }

    /// <summary>
    /// GetTransporter method retrieves a transporter by the device name, it gets created if it doesn't exist
    /// </summary>
    /// <param name="device">The ProcessDeviceDto object</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the TransporterVm object</returns>
    private async Task<TransporterVm> GetTransporter(ProcessDeviceDto device, CancellationToken cancellationToken)
    {
        var transporter = await transporterReader.GetTransporterAsync(device.Name, cancellationToken);
        if (transporter == default)
        {
            transporter = await transporterWriter.CreateTransporterAsync(
                new TransporterDto(
                    device.Name,
                    device.TransporterTypeId),
                cancellationToken);
        }

        return transporter;
    }
}
