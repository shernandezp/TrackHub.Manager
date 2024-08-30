﻿using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

// This class represents a reader for retrieving operator data from the application database.
public sealed class OperatorReader(IApplicationDbContext context) : IOperatorReader
{
    // Retrieves an operator by its ID asynchronously.
    // Parameters:
    // - id: The ID of the operator to retrieve.
    // - cancellationToken: A cancellation token to cancel the operation if needed.
    // Returns:
    // - An instance of OperatorVm representing the retrieved operator.
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

    // Retrieves a collection of operators by account ID asynchronously.
    // Parameters:
    // - accountId: The ID of the account to retrieve operators for.
    // - cancellationToken: A cancellation token to cancel the operation if needed.
    // Returns:
    // - A collection of OperatorVm instances representing the retrieved operators.
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
                o.LastModified,
                null))
            .ToListAsync(cancellationToken);

    // Retrieves a collection of operators by user ID asynchronously.
    // Parameters:
    // - userId: The ID of the user to retrieve operators for.
    // - cancellationToken: A cancellation token to cancel the operation if needed.
    // Returns:
    // - A collection of OperatorVm instances representing the retrieved operators.
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