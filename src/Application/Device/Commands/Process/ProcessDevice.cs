// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
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

public class ProcessDeviceCommandHandler(
    IPublisher publisher,
    IDeviceReader deviceReader,
    IOperatorReader operatorReader,
    ITransporterWriter transporterWriter,
    ITransporterReader transporterReader) : IRequestHandler<ProcessDeviceCommand, bool>
{

    public async Task<bool> Handle(ProcessDeviceCommand request, CancellationToken cancellationToken)
    {
        var @operator = await operatorReader.GetOperatorAsync(request.OperatorId, cancellationToken);
        var transporter = await GetTransporter(request.ProcessDevice, @operator.AccountId, cancellationToken);
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
                request.OperatorId,
                @operator.AccountId);
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

    private async Task<TransporterVm> GetTransporter(ProcessDeviceDto device, Guid accountId, CancellationToken cancellationToken)
    {
        var transporter = await transporterReader.GetTransporterAsync(device.Name, cancellationToken);
        if (transporter == default)
        {
            transporter = await transporterWriter.CreateTransporterAsync(
                new TransporterDto(
                    device.Name,
                    device.TransporterTypeId,
                    accountId),
                cancellationToken);
        }

        return transporter;
    }
}
