namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class GroupWriter(IApplicationDbContext context) : IGroupWriter
{
    public async Task<GroupVm> CreateGroupAsync(GroupDto groupDto, CancellationToken cancellationToken)
    {
        var group = new Group(
            groupDto.Name,
            groupDto.Description,
            groupDto.IsMaster,
            groupDto.Active,
            groupDto.AccountId);

        await context.Groups.AddAsync(group, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new GroupVm(
            group.GroupId,
            group.Name,
            group.Description,
            group.IsMaster,
            group.Active,
            group.AccountId);
    }

    public async Task UpdateGroupAsync(UpdateGroupDto groupDto, CancellationToken cancellationToken)
    {
        var group = await context.Groups.FindAsync([groupDto.GroupId], cancellationToken)
            ?? throw new NotFoundException(nameof(Group), $"{groupDto.GroupId}");

        group.Name = groupDto.Name;
        group.Description = groupDto.Description;
        group.IsMaster = groupDto.IsMaster;
        group.Active = groupDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteGroupAsync(Guid groupId, CancellationToken cancellationToken)
    {
        var group = await context.Groups.FindAsync([groupId], cancellationToken)
            ?? throw new NotFoundException(nameof(Group), $"{groupId}");

        if (group.IsMaster)
            throw new InvalidOperationException("Master group cannot be deleted.");

        context.Groups.Remove(group);
        await context.SaveChangesAsync(cancellationToken);
    }
}
