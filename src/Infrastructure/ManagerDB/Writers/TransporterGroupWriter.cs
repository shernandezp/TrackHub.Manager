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

using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.Writers;

// TransporterGroupWriter class for writing transporter group data
public sealed class TransporterGroupWriter(IApplicationDbContext context) : ITransporterGroupWriter
{
    /// <summary>
    /// Create a new transporter group asynchronously.
    /// </summary>
    /// <param name="transporterGroupDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created transporter group view model</returns>
    public async Task<TransporterGroupVm> CreateTransporterGroupAsync(TransporterGroupDto transporterGroupDto, CancellationToken cancellationToken)
    {
        var transporterGroup = new TransporterGroup
        {
            TransporterId = transporterGroupDto.TransporterId,
            GroupId = transporterGroupDto.GroupId
        };

        await context.TransportersGroup.AddAsync(transporterGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TransporterGroupVm(
            transporterGroup.TransporterId,
            transporterGroup.GroupId);
    }

    /// <summary>
    /// Delete a transporter group asynchronously.
    /// </summary>
    /// <param name="transporterId"></param>
    /// <param name="groupId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteTransporterGroupAsync(Guid transporterId, long groupId, CancellationToken cancellationToken)
    {
        var transporterGroup = await context.TransportersGroup.FindAsync([groupId, transporterId], cancellationToken);

        if (transporterGroup != default)
        {
            context.TransportersGroup.Attach(transporterGroup);

            context.TransportersGroup.Remove(transporterGroup);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Delete all transporter groups that match the given transporterId asynchronously.
    /// </summary>
    /// <param name="transporterId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task DeleteTransporterGroupsAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        var transporterGroups = await context.TransportersGroup
            .Where(tg => tg.TransporterId == transporterId)
            .ToListAsync(cancellationToken);

        if (transporterGroups.Count != 0)
        {
            context.TransportersGroup.AttachRange(transporterGroups);

            context.TransportersGroup.RemoveRange(transporterGroups);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
