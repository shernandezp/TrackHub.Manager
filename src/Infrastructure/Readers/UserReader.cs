namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class UserReader(IApplicationDbContext context) : IUserReader
{
    public async Task<UserVm> GetUserAsync(Guid id, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId.Equals(id))
            .Select(u => new UserVm(
                u.UserId,
                u.Username,
                u.Active,
                u.AccountId))
            .FirstAsync(cancellationToken);

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
