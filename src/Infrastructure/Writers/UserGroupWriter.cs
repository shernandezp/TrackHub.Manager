namespace TrackHub.Manager.Infrastructure.Writers;

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

        await context.UserGroups.AddAsync(userGroup, cancellationToken);
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
        var userGroup = await context.UserGroups.FindAsync([userId, groupId], cancellationToken)
            ?? throw new NotFoundException(nameof(UserGroup), $"{userId},{groupId}");

        context.UserGroups.Remove(userGroup);
        await context.SaveChangesAsync(cancellationToken);
    }
}
