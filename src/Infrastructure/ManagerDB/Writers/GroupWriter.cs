﻿namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

/// <summary>
/// Writer class for managing groups.
/// </summary>
public sealed class GroupWriter(IApplicationDbContext context) : IGroupWriter
{
    /// <summary>
    /// Creates a new group.
    /// </summary>
    /// <param name="groupDto">The group data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The created group view model.</returns>
    public async Task<GroupVm> CreateGroupAsync(GroupDto groupDto, Guid accountId, CancellationToken cancellationToken)
    {
        var group = new Group(
            groupDto.Name,
            groupDto.Description,
            groupDto.Active,
            accountId);

        await context.Groups.AddAsync(group, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new GroupVm(
            group.GroupId,
            group.Name,
            group.Description,
            group.Active,
            group.AccountId);
    }

    /// <summary>
    /// Updates an existing group.
    /// </summary>
    /// <param name="groupDto">The updated group data transfer object.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task UpdateGroupAsync(UpdateGroupDto groupDto, CancellationToken cancellationToken)
    {
        var group = await context.Groups.FindAsync([groupDto.GroupId], cancellationToken)
            ?? throw new NotFoundException(nameof(Group), $"{groupDto.GroupId}");

        context.Groups.Attach(group);

        group.Name = groupDto.Name;
        group.Description = groupDto.Description;
        group.Active = groupDto.Active;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a group.
    /// </summary>
    /// <param name="groupId">The ID of the group to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    public async Task DeleteGroupAsync(long groupId, CancellationToken cancellationToken)
    {
        var group = await context.Groups.FindAsync([groupId], cancellationToken)
            ?? throw new NotFoundException(nameof(Group), $"{groupId}");

        context.Groups.Attach(group);

        context.Groups.Remove(group);
        await context.SaveChangesAsync(cancellationToken);
    }
}
