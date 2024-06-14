namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class GroupReader(IApplicationDbContext context) : IGroupReader
{
    public async Task<GroupVm> GetGroupAsync(Guid id, CancellationToken cancellationToken)
        => await context.Groups
            .Where(d => d.GroupId.Equals(id))
            .Select(d => new GroupVm(
                d.GroupId,
                d.Name,
                d.Description,
                d.IsMaster,
                d.Active,
                d.AccountId))
            .FirstAsync(cancellationToken);

    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .Distinct()
            .Select(d => new GroupVm(
                d.GroupId,
                d.Name,
                d.Description,
                d.IsMaster,
                d.Active,
                d.AccountId))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId == accountId)
            .SelectMany(a => a.Groups)
            .Distinct()
            .Select(d => new GroupVm(
                d.GroupId,
                d.Name,
                d.Description,
                d.IsMaster,
                d.Active,
                d.AccountId))
            .ToListAsync(cancellationToken);

}
