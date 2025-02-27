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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// This class represents a writer for UserGroup entities in the infrastructure layer.
public sealed class UserGroupWriter(IApplicationDbContext context) : IUserGroupWriter
{
    // Creates a new UserGroup asynchronously.
    // Parameters:
    //   userGroupDto: The UserGroupDto object containing the data for the new UserGroup.
    //   cancellationToken: The cancellation token.
    // Returns:
    //   A Task that represents the asynchronous operation. The task result contains the created UserGroupVm.
    public async Task<UserGroupVm> CreateUserGroupAsync(UserGroupDto userGroupDto, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroup
        {
            UserId = userGroupDto.UserId,
            GroupId = userGroupDto.GroupId
        };

        await context.UsersGroup.AddAsync(userGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new UserGroupVm(
            userGroup.UserId,
            userGroup.GroupId);
    }

    // Deletes a UserGroup asynchronously.
    // Parameters:
    //   userId: The ID of the user associated with the UserGroup.
    //   groupId: The ID of the group associated with the UserGroup.
    //   cancellationToken: The cancellation token.
    // Throws:
    //   NotFoundException: If the UserGroup with the specified userId and groupId is not found.
    public async Task DeleteUserGroupAsync(Guid userId, long groupId, CancellationToken cancellationToken)
    {
        var userGroup = await context.UsersGroup.FindAsync([groupId, userId], cancellationToken)
            ?? throw new NotFoundException(nameof(UserGroup), $"{userId},{groupId}");

        context.UsersGroup.Attach(userGroup);

        context.UsersGroup.Remove(userGroup);
        await context.SaveChangesAsync(cancellationToken);
    }
}
