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

using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class PointOfInterestReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IPointOfInterestReader
{
    public async Task<PointOfInterestVm> GetPointOfInterestAsync(Guid id, CancellationToken cancellationToken)
    {
        var poi = await Context.PointsOfInterest
            .Where(p => p.PointOfInterestId == id)
            .Select(p => new PointOfInterestVm(
                p.PointOfInterestId,
                p.AccountId,
                p.Name,
                p.Description,
                p.Type,
                p.Latitude,
                p.Longitude,
                p.Address,
                p.Color,
                p.GroupId,
                p.Active))
            .FirstAsync(cancellationToken);

        RequireAccountAccess(poi.AccountId);
        return poi;
    }

    // When visibleToUserId is set, POIs restricted to a Group outside that user's groups
    // are excluded; POIs with no Group are visible to every account user with read access.
    public async Task<IReadOnlyCollection<PointOfInterestVm>> GetPointsOfInterestByAccountAsync(Guid accountId, Guid? visibleToUserId, CancellationToken cancellationToken)
    {
        var scopedAccountId = RequireAccountAccess(accountId);

        var query = Context.PointsOfInterest.Where(p => p.AccountId == scopedAccountId);

        if (visibleToUserId.HasValue)
        {
            var userId = visibleToUserId.Value;
            var userGroupIds = Context.Users
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.Groups.Select(g => g.GroupId));

            query = query.Where(p => p.GroupId == null || userGroupIds.Contains(p.GroupId.Value));
        }

        return await query
            .OrderBy(p => p.Name)
            .Select(p => new PointOfInterestVm(
                p.PointOfInterestId,
                p.AccountId,
                p.Name,
                p.Description,
                p.Type,
                p.Latitude,
                p.Longitude,
                p.Address,
                p.Color,
                p.GroupId,
                p.Active))
            .ToListAsync(cancellationToken);
    }
}
