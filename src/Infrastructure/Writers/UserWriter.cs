namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class UserWriter(IApplicationDbContext context) : IUserWriter
{
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

    public async Task UpdateUserAsync(UpdateUserDto userDto, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([userDto.UserId], cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"{userDto.UserId}");

        user.Username = userDto.Username;
        user.Active = userDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken)
            ?? throw new NotFoundException(nameof(User), $"{userId}");

        context.Users.Remove(user);
        await context.SaveChangesAsync(cancellationToken);
    }
}
