using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// This class represents a reader for retrieving operator data from the application database.
public sealed class OperatorReader(IApplicationDbContext context) : IOperatorReader
{

    /// <summary>
    /// Retrieves an operator by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the operator to retrieve.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>An instance of OperatorVm representing the retrieved operator.</returns>
    public async Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken)
        => await context.Operators
            .Include(o => o.Credential)
            .Where(o => o.OperatorId.Equals(id))
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                (ProtocolType)o.ProtocolType,
                o.ProtocolType,
                o.AccountId,
                o.LastModified,
                o.Credential == null ? null : new CredentialTokenVm(
                    o.Credential.CredentialId,
                    o.Credential.Uri,
                    o.Credential.Username,
                    o.Credential.Password,
                    o.Credential.Salt,
                    o.Credential.Key,
                    o.Credential.Key2,
                    o.Credential.Token,
                    o.Credential.TokenExpiration,
                    o.Credential.RefreshToken,
                    o.Credential.RefreshTokenExpiration)))
            .FirstAsync(cancellationToken);

    /// <summary>
    /// Retrieves a collection of operators by account ID asynchronously.
    /// </summary>
    /// <param name="accountId">The ID of the account to retrieve operators for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>A collection of OperatorVm instances representing the retrieved operators.</returns>
    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Operators
            .Where(o => o.AccountId == accountId)
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                (ProtocolType)o.ProtocolType,
                o.ProtocolType,
                o.AccountId,
                o.LastModified,
                null))
            .ToListAsync(cancellationToken);

    /// <summary>
    /// Retrieves a collection of operators by user ID asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve operators for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>A collection of OperatorVm instances representing the retrieved operators.</returns>
    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.UsersGroup
            .Where(ug => ug.UserId == userId)
            .SelectMany(ug => ug.Group.Transporters)
            .SelectMany(d => d.Devices)
            .Select(d => d.Operator)
            .Distinct()
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                (ProtocolType)o.ProtocolType,
                o.ProtocolType,
                o.AccountId,
                o.LastModified,
                o.Credential == null ? null : new CredentialTokenVm(
                    o.Credential.CredentialId,
                    o.Credential.Uri,
                    o.Credential.Username,
                    o.Credential.Password,
                    o.Credential.Salt,
                    o.Credential.Key,
                    o.Credential.Key2,
                    o.Credential.Token,
                    o.Credential.TokenExpiration,
                    o.Credential.RefreshToken,
                    o.Credential.RefreshTokenExpiration)))
            .ToListAsync(cancellationToken);

}
