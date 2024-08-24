﻿using TrackHub.Manager.Application.Device.Events;

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
    // Handles the ProcessDeviceCommand
    // Parameters:
    // - request: The ProcessDeviceCommand object
    // - cancellationToken: A token to cancel the operation if needed
    // Returns:
    // - A Task representing the asynchronous operation. The task result contains a boolean value indicating the success of the operation
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

    // GetTransporter method retrieves a transporter by the device name, it gets created if it doesn't exist
    // Parameters:
    // - device: The ProcessDeviceDto object
    // - cancellationToken: A token to cancel the operation if needed
    // Returns:
    // - A Task representing the asynchronous operation. The task result contains the TransporterVm object
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
