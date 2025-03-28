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

using Common.Domain.Enums;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// This class represents a writer for Device entities
public sealed class DeviceWriter(IApplicationDbContext context) : IDeviceWriter
{
    /// <summary>
    /// Creates a new Device asynchronously
    /// </summary>
    /// <param name="deviceDto">The DTO object containing the device data</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns>A Task representing the asynchronous operation. The task result contains the created DeviceVm</returns>
    public async Task<DeviceVm> CreateDeviceAsync(DeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = new Device
        (
            deviceDto.Name,
            deviceDto.Identifier,
            deviceDto.Serial,
            deviceDto.DeviceTypeId,
            deviceDto.Description,
            deviceDto.TransporterId,
            deviceDto.OperatorId
        );

        await context.Devices.AddAsync(device, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new DeviceVm(
            device.DeviceId,
            device.Name,
            device.Identifier,
            device.Serial,
            (DeviceType)device.DeviceTypeId,
            device.DeviceTypeId,
            device.Description,
            device.TransporterId,
            device.OperatorId);
    }

    /// <summary>
    /// Updates a Device asynchronously
    /// </summary>
    /// <param name="deviceDto">The DTO object containing the updated device data</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">If the Device with the specified IDs is not found</exception>
    public async Task UpdateDeviceAsync(UpdateDeviceDto deviceDto, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync(deviceDto.DeviceId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{deviceDto.DeviceId}");

        context.Devices.Attach(device);

        device.Name = deviceDto.Name;
        device.Identifier = deviceDto.Identifier;
        device.DeviceTypeId = deviceDto.DeviceTypeId;
        device.Description = deviceDto.Description;
        device.TransporterId = deviceDto.TransporterId;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a Device asynchronously
    /// </summary>
    /// <param name="deviceId">The ID of the device associated with the Device</param>
    /// <param name="cancellationToken">A token to cancel the operation if needed</param>
    /// <returns></returns>
    /// <exception cref="NotFoundException">If the Device with the specified IDs is not found</exception>
    public async Task DeleteDeviceAsync(Guid deviceId, CancellationToken cancellationToken)
    {
        var device = await context.Devices.FindAsync([deviceId], cancellationToken)
            ?? throw new NotFoundException(nameof(Device), $"{deviceId}");

        context.Devices.Attach(device);

        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}
