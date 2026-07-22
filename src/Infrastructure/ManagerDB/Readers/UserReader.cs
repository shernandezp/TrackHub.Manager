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

// This class represents a reader for retrieving user data from the database.
//
// Tenant scoping: the by-id and by-group reads take a caller-supplied key, and their queries carry no
// top-level AccountId, so AccountScopeBehavior has no account to compare and this DbContext has no
// global query filter. Scoping happens here, where the row's owning account first becomes known.
// RequireAccountAccess admits a global service client and an active support grant, so internal and
// support flows are unaffected.
public sealed class UserReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), IUserReader
{
    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>A UserVm object representing the retrieved user.</returns>
    public async Task<UserVm> GetUserAsync(Guid id, CancellationToken cancellationToken)
    {
        var user = await Context.Users
            .Where(u => u.UserId.Equals(id))
            .Select(u => new UserVm(
                u.UserId,
                u.Username,
                u.Active,
                u.AccountId))
            .FirstAsync(cancellationToken);

        RequireAccountAccess(user.AccountId);
        return user;
    }

    /// <summary>
    /// Retrieves a collection of users by their account ID.
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An IReadOnlyCollection<UserVm> representing the retrieved users.
    /// </returns>
    public async Task<IReadOnlyCollection<UserVm>> GetUsersByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await Context.Users
            .Where(u => u.AccountId == accountId)
            .Select(u => new UserVm(
                u.UserId,
                u.Username,
                u.Active,
                u.AccountId))
            .Distinct()
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieves a collection of users by their group ID.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>
    /// An IReadOnlyCollection<UserVm> representing the retrieved users.
    /// </returns>
    public async Task<IReadOnlyCollection<UserVm>> GetUsersByGroupAsync(long groupId, CancellationToken cancellationToken)
    {
        // GroupId is a sequential bigint and therefore directly enumerable, so the group's owning
        // account is resolved first and authorized before any user row is projected.
        var groupAccountId = await Context.Groups
            .Where(g => g.GroupId == groupId)
            .Select(g => (Guid?)g.AccountId)
            .FirstOrDefaultAsync(cancellationToken);

        if (!groupAccountId.HasValue)
        {
            return [];
        }

        RequireAccountAccess(groupAccountId.Value);

        return await Context.Groups
            .Where(g => g.GroupId == groupId)
            .SelectMany(g => g.Users)
            .Select(u => new UserVm(
                u.UserId,
                u.Username,
                u.Active,
                u.AccountId))
            .Distinct()
            .ToListAsync(cancellationToken);
    }
}
