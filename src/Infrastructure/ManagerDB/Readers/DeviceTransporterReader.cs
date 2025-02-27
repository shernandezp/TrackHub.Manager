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
using Common.Domain.Helpers;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class DeviceTransporterReader(IApplicationDbContext context) : IDeviceTransporterReader
{

    /// <summary>
    /// Retrieves a collection of devices by user ID and operator ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="operatorId">The ID of the operator</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<IReadOnlyCollection<DeviceTransporterVm>>: A task that represents the asynchronous operation. The task result contains a collection of DeviceVm objects.</returns>
    public async Task<IReadOnlyCollection<DeviceTransporterVm>> GetDeviceTransporterByUserAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(d => d.Devices)
            .Where(d => d.OperatorId == operatorId && d.TransporterId != null)
            .Select(d => new DeviceTransporterVm(
                d.TransporterId!.Value,
                d.Identifier,
                d.Serial,
                d.Transporter.Name,
                (TransporterType)d.Transporter.TransporterTypeId,
                d.Transporter.TransporterTypeId))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieve a collection of devices by filters
    /// </summary>
    /// <param name="filters">The filters to apply</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of DeviceTransporterVm objects</returns>
    public async Task<IReadOnlyCollection<DeviceTransporterVm>> GetDeviceTransportersAsync(Filters filters, CancellationToken cancellationToken)
    {
        var query = context.Devices.AsQueryable();
        query = filters.Apply(query);

        return await query
            .Where(d => d.TransporterId != null)
            .Select(d => new DeviceTransporterVm(
                d.TransporterId!.Value,
                d.Identifier,
                d.Serial,
                d.Transporter.Name,
                (TransporterType)d.Transporter.TransporterTypeId,
                d.Transporter.TransporterTypeId))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a DeviceTransporterVm by transporter ID
    /// </summary>
    /// <param name="transporterId">The ID of the transporter</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>Task<DeviceTransporterVm>: A task that represents the asynchronous operation. The task result contains the DeviceTransporterVm object.</returns>
    public async Task<DeviceTransporterVm> GetDeviceTransporterAsync(Guid transporterId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.TransporterId == transporterId)
            .Select(d => new DeviceTransporterVm(
                d.TransporterId!.Value,
                d.Identifier,
                d.Serial,
                d.Transporter.Name,
                (TransporterType)d.Transporter.TransporterTypeId,
                d.Transporter.TransporterTypeId))
            .FirstAsync(cancellationToken);

}
