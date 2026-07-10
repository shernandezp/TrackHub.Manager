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

namespace TrackHub.Manager.Application.PointsOfInterest.Queries.GetByAccount;

[Authorize(Resource = Resources.PointsOfInterest, Action = Actions.Read)]
public readonly record struct GetPointsOfInterestByAccountQuery() : IRequest<IReadOnlyCollection<PointOfInterestVm>>;

public class GetPointsOfInterestByAccountQueryHandler(
    IPointOfInterestReader reader,
    IUserReader userReader,
    ICurrentPrincipal principal,
    IUser user) : IRequestHandler<GetPointsOfInterestByAccountQuery, IReadOnlyCollection<PointOfInterestVm>>
{
    private Guid UserId { get; } = Guid.TryParse(user.Id, out var userId) ? userId : throw new UnauthorizedAccessException();

    public async Task<IReadOnlyCollection<PointOfInterestVm>> Handle(GetPointsOfInterestByAccountQuery request, CancellationToken cancellationToken)
    {
        var userVm = await userReader.GetUserAsync(UserId, cancellationToken);

        // Administrators and managers see every POI in the account; other users only see
        // POIs without a group restriction or restricted to one of their own groups.
        var isPrivileged = string.Equals(principal.Role, Roles.Administrator, StringComparison.OrdinalIgnoreCase)
            || string.Equals(principal.Role, Roles.Manager, StringComparison.OrdinalIgnoreCase);

        return await reader.GetPointsOfInterestByAccountAsync(userVm.AccountId, isPrivileged ? null : UserId, cancellationToken);
    }
}
