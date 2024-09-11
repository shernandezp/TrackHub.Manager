﻿namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// This class represents a reader for retrieving group information from the database.
public sealed class GroupReader(IApplicationDbContext context) : IGroupReader
{
    // Retrieves a specific group by its ID.
    // Parameters:
    //   id: The ID of the group to retrieve.
    //   cancellationToken: A token to cancel the operation.
    // Returns:
    //   A Task that represents the asynchronous operation. The task result contains the GroupVm object.
    public async Task<GroupVm> GetGroupAsync(long id, CancellationToken cancellationToken)
        => await context.Groups
            .Where(d => d.GroupId.Equals(id))
            .Select(d => new GroupVm(
                d.GroupId,
                d.Name,
                d.Description,
                d.Active,
                d.AccountId))
            .FirstAsync(cancellationToken);

    // Retrieves all groups associated with a specific user.
    // Parameters:
    //   userId: The ID of the user.
    //   cancellationToken: A token to cancel the operation.
    // Returns:
    //   A Task that represents the asynchronous operation. The task result contains a collection of GroupVm objects.
    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .Distinct()
            .Select(d => new GroupVm(
                d.GroupId,
                d.Name,
                d.Description,
                d.Active,
                d.AccountId))
            .ToListAsync(cancellationToken);

    // Retrieves all groups associated with a specific account.
    // Parameters:
    //   accountId: The ID of the account.
    //   cancellationToken: A token to cancel the operation.
    // Returns:
    //   A Task that represents the asynchronous operation. The task result contains a collection of GroupVm objects.
    public async Task<IReadOnlyCollection<GroupVm>> GetGroupsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId == accountId)
            .SelectMany(a => a.Groups)
            .Distinct()
            .Select(d => new GroupVm(
                d.GroupId,
                d.Name,
                d.Description,
                d.Active,
                d.AccountId))
            .ToListAsync(cancellationToken);

}
