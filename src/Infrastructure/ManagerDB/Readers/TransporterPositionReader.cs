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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class TransporterPositionReader(IApplicationDbContext context) : ITransporterPositionReader
{
    /// <summary>
    /// Retrieve a collection of transporter positions by user ID and operator ID asynchronously. 
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve transporter positions for</param>
    /// <param name="operatorId">The ID of the operator to retrieve transporter positions for</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed</param>
    /// <returns>A collection of TransporterPositionVm instances representing the retrieved transporter positions</returns>
    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid userId, Guid operatorId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(t => t.Devices)
            .Where(d => d.OperatorId == operatorId)
            .Select(d => d.Transporter.Position)
            .Where(tp => tp != null)
            .Select(tp => new TransporterPositionVm(
                tp!.TransporterPositionId,
                tp.TransporterId,
                tp.Transporter.Name,
                (TransporterType)tp.Transporter.TransporterTypeId,
                tp.GeometryId,
                tp.Latitude,
                tp.Longitude,
                tp.Altitude,
                new(DateTime.SpecifyKind(tp.DateTime, DateTimeKind.Utc), DateTimeKind.Utc == DateTime.SpecifyKind(tp.DateTime, DateTimeKind.Utc).Kind ? TimeSpan.Zero : tp.Offset),
                tp.Speed,
                tp.Course,
                tp.EventId,
                tp.Address,
                tp.City,
                tp.State,
                tp.Country,
                tp.Attributes))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieve a collection of transporter positions by operator ID asynchronously.
    /// </summary>
    /// <param name="operatorId">The ID of the operator to retrieve transporter positions for</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed</param>
    /// <returns>A collection of TransporterPositionVm instances representing the retrieved transporter positions</returns>
    public async Task<IReadOnlyCollection<TransporterPositionVm>> GetTransporterPositionsAsync(Guid operatorId, CancellationToken cancellationToken)
        => await context.Devices
            .Where(d => d.OperatorId == operatorId)
            .Select(d => d.Transporter.Position)
            .Where(tp => tp != null)
            .Select(tp => new TransporterPositionVm(
                tp!.TransporterPositionId,
                tp.TransporterId,
                tp.Transporter.Name,
                (TransporterType)tp.Transporter.TransporterTypeId,
                tp.GeometryId,
                tp.Latitude,
                tp.Longitude,
                tp.Altitude,
                new(DateTime.SpecifyKind(tp.DateTime, DateTimeKind.Utc), tp.Offset),
                tp.Speed,
                tp.Course,
                tp.EventId,
                tp.Address,
                tp.City,
                tp.State,
                tp.Country,
                tp.Attributes))
            .ToListAsync(cancellationToken);

}
