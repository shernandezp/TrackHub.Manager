namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// This class represents a reader for retrieving user data from the database.
public sealed class UserReader(IApplicationDbContext context) : IUserReader
{
    // Retrieves a user by their ID.
    // Parameters:
    //   id: The ID of the user to retrieve.
    //   cancellationToken: A token to cancel the operation if needed.
    // Returns:
    //   A UserVm object representing the retrieved user.
    public async Task<UserVm> GetUserAsync(Guid id, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId.Equals(id))
            .Select(u => new UserVm(
                u.UserId,
                u.Username,
                u.Active,
                u.AccountId))
            .FirstAsync(cancellationToken);

    // Retrieves a collection of users by their account ID.
    // Parameters:
    //   accountId: The ID of the account to retrieve users for.
    //   cancellationToken: A token to cancel the operation if needed.
    // Returns:
    //   An IReadOnlyCollection<UserVm> representing the retrieved users.
    public async Task<IReadOnlyCollection<UserVm>> GetUsersByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.AccountId == accountId)
            .Select(u => new UserVm(
                u.UserId,
                u.Username,
                u.Active,
                u.AccountId))
            .Distinct()
            .ToListAsync(cancellationToken);
}
