// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using Common.Domain.Enums;
using Common.Domain.Helpers;
using TrackHub.Manager.Infrastructure.Interfaces;

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
    /// <param name="filters">Filters.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>A collection of OperatorVm instances representing the retrieved operators.</returns>
    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsAsync(Filters filters, CancellationToken cancellationToken)
    {
        var query = context.Operators.AsQueryable();
        query = filters.Apply(query);

        return await query
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
    }

    /// <summary>
    /// Retrieves a collection of operators by user ID asynchronously.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve operators for.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation if needed.</param>
    /// <returns>A collection of OperatorVm instances representing the retrieved operators.</returns>
    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        // Resolve user's account to filter operators by account ownership
        var accountId = await context.Users
            .Where(u => u.UserId == userId)
            .Select(u => u.AccountId)
            .FirstAsync(cancellationToken);

        return await context.Operators
            .Where(o => o.AccountId == accountId 
                && o.Devices.Any(d => d.Transporter != null 
                    && d.Transporter.Groups.Any(g => g.Users.Any(u => u.UserId == userId))))
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

    /// <summary>
    /// Retrieves an operator by transporter ID asynchronously.
    /// </summary>
    /// <param name="transporterId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>An OperatorVm instance representing the retrieved operator.</returns>
    public async Task<OperatorVm> GetOperatorByTransporterAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        // First, get the account ID from the transporter's group
        var accountId = await context.Transporters
            .Where(t => t.TransporterId == transporterId)
            .SelectMany(t => t.Groups)
            .Select(g => g.AccountId)
            .FirstAsync(cancellationToken);

        // Then, get the operator that belongs to the same account
        return await context.Transporters
            .Where(t => t.TransporterId == transporterId)
            .SelectMany(t => t.Devices)
            .Select(d => d.Operator)
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
            .FirstOrDefaultAsync(cancellationToken);
    }

}
