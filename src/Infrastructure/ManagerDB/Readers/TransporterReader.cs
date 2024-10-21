﻿using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// TransporterReader class for reading transporter information
public sealed class TransporterReader(IApplicationDbContext context) : ITransporterReader
{
    /// <summary>
    /// GetTransporterAsync method retrieves a transporter by its ID 
    /// </summary>
    /// <param name="id">The ID of the transporter</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TransporterVm object.</returns>
    public async Task<TransporterVm> GetTransporterAsync(Guid id, CancellationToken cancellationToken)
        => await context.Transporters
            .Where(d => d.TransporterId.Equals(id))
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId))
            .FirstAsync(cancellationToken);

    /// <summary>
    /// GetTransporterAsync method retrieves a transporter by its name
    /// </summary>
    /// <param name="name">The name of the transporter</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the TransporterVm object.</returns>
    public async Task<TransporterVm> GetTransporterAsync(string name, CancellationToken cancellationToken)
        => await context.Transporters
            .Where(d => d.Name.Equals(name))
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId))
            .FirstOrDefaultAsync(cancellationToken);

    /// <summary>
    /// GetTransportersByGroupAsync method retrieves transporters by group ID
    /// </summary>
    /// <param name="groupId">The ID of the group</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TransporterVm objects.</returns>
    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByGroupAsync(long groupId, CancellationToken cancellationToken)
        => await context.Groups
            .Where(g => g.GroupId == groupId)
            .SelectMany(g => g.Transporters)
            .Select(d => new TransporterVm(
                d.TransporterId,
                d.Name,
                (TransporterType)d.TransporterTypeId,
                d.TransporterTypeId))
            .Distinct()
            .ToListAsync(cancellationToken);

    /// <summary>
    /// GetTransportersByUserAsync method retrieves transporters by user ID
    /// </summary>
    /// <param name="userId">The ID of the user</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TransporterVm objects.</returns>
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
                d.TransporterTypeId))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// GetTransportersByAccountAsync method retrieves transporters by account ID
    /// </summary>
    /// <param name="accountId">The ID of the account</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of TransporterVm objects.</returns>
    public async Task<IReadOnlyCollection<TransporterVm>> GetTransportersByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Accounts
            .Where(a => a.AccountId == accountId)
            .SelectMany(a => a.Operators)
            .SelectMany(o => o.Devices)
            .Select(d => d.Transporter)
            .Distinct()
            .Select(t => new TransporterVm(
                t.TransporterId,
                t.Name,
                (TransporterType)t.TransporterTypeId,
                t.TransporterTypeId))
            .ToListAsync(cancellationToken);


}
