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

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// UserWriter class for handling user-related operations
public sealed class UserWriter(IApplicationDbContext context) : IUserWriter
{
    /// <summary>
    /// Creates a new user asynchronously
    /// </summary>
    /// <param name="userDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created user view model</returns>
    public async Task<UserVm> CreateUserAsync(UserDto userDto, CancellationToken cancellationToken)
    {
        var user = new User(
            userDto.UserId,
            userDto.Username,
            userDto.Active,
            userDto.AccountId);

        await context.Users.AddAsync(user, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new UserVm(
            user.UserId,
            user.Username,
            user.Active,
            user.AccountId);
    }

    /// <summary>
    /// Updates an existing user asynchronously
    /// </summary>
    /// <param name="userDto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateUserAsync(UpdateUserDto userDto, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([userDto.UserId], cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"{userDto.UserId}");

        context.Users.Attach(user);

        user.Username = userDto.Username;
        user.Active = userDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a user asynchronously
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="NotFoundException"></exception>
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"{userId}");

        context.Users.Attach(user);

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}
