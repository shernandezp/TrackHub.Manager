using TrackHub.Manager.Infrastructure.ManagerDB.Entities;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// UserWriter class for handling user-related operations
public sealed class UserWriter(IApplicationDbContext context) : IUserWriter
{
    // Creates a new user asynchronously
    // Parameters:
    // - userDto: The user data transfer object
    // - cancellationToken: The cancellation token
    // Returns:
    // - The created user view model
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

    // Updates an existing user asynchronously
    // Parameters:
    // - userDto: The updated user data transfer object
    // - cancellationToken: The cancellation token
    public async Task UpdateUserAsync(UpdateUserDto userDto, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([userDto.UserId], cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"{userDto.UserId}");

        context.Users.Attach(user);

        user.Username = userDto.Username;
        user.Active = userDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes a user asynchronously
    // Parameters:
    // - userId: The ID of the user to delete
    // - cancellationToken: The cancellation token
    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"{userId}");

        context.Users.Attach(user);

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}
