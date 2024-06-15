namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class UserGroupWriter(IApplicationDbContext context) : IUserGroupWriter
{
    public async Task<UserGroupVm> CreateUserGroupAsync(UserGroupDto userGroupDto, CancellationToken cancellationToken)
    {
        var userGroup = new UserGroup
        {
            UserId = userGroupDto.UserId,
            GroupId = userGroupDto.GroupId
        };

        await context.UserGroups.AddAsync(userGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new UserGroupVm(
            userGroup.UserId,
            userGroup.GroupId);
    }

    public async Task DeleteUserGroupAsync(Guid userId, long groupId, CancellationToken cancellationToken)
    {
        var userGroup = await context.UserGroups.FindAsync([userId, groupId], cancellationToken)
            ?? throw new NotFoundException(nameof(UserGroup), $"{userId},{groupId}");

        context.UserGroups.Remove(userGroup);
        await context.SaveChangesAsync(cancellationToken);
    }
}
