using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;

// TransporterReader class for reading transporter information
public sealed class TransporterReader(IApplicationDbContext context) : ITransporterReader
{
    // GetTransporterAsync method retrieves a transporter by its ID
    // Parameters:
    // - id: The ID of the transporter
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<TransporterVm>: A task that represents the asynchronous operation. The task result contains the TransporterVm object.
    public async Task<TransporterVm> GetTransporterAsync(Guid id, CancellationToken cancellationToken)
        => await context.Transporters
            .Where(d => d.TransporterId.Equals(id))
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId,
                d.Icon))
            .FirstAsync(cancellationToken);

    // GetTransportersByGroupAsync method retrieves transporters by group ID
    // Parameters:
    // - groupId: The ID of the group
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<TransporterVm>>: A task that represents the asynchronous operation. The task result contains a collection of TransporterVm objects.
    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByGroupAsync(long groupId, CancellationToken cancellationToken)
        => await context.Groups
            .Where(g => g.GroupId == groupId)
            .SelectMany(g => g.Transporters)
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId,
                d.Icon))
            .Distinct()
            .ToListAsync(cancellationToken);

    // GetTransportersByUserAsync method retrieves transporters by user ID
    // Parameters:
    // - userId: The ID of the user
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<TransporterVm>>: A task that represents the asynchronous operation. The task result contains a collection of TransporterVm objects.
    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .SelectMany(g => g.Transporters)
            .Distinct()
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId,
                d.Icon))
            .ToListAsync(cancellationToken);

    // GetTransportersByAccountAsync method retrieves transporters by account ID
    // Parameters:
    // - accountId: The ID of the account
    // - cancellationToken: A cancellation token to cancel the operation
    // Returns:
    // - Task<IReadOnlyCollection<TransporterVm>>: A task that represents the asynchronous operation. The task result contains a collection of TransporterVm objects.
    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId == accountId)
            .SelectMany(a => a.Groups)
            .SelectMany(g => g.Transporters)
            .Distinct()
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId,
                d.Icon))
            .ToListAsync(cancellationToken);


}
