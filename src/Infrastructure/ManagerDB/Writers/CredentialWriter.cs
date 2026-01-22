// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
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

using Common.Domain.Extensions;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

// This class represents a writer for credentials in the infrastructure layer.
public sealed class CredentialWriter(IApplicationDbContext context) : ICredentialWriter
{

    /// <summary>
    /// Creates a new credential asynchronously.
    /// </summary>
    /// <param name="credentialDto"></param>
    /// <param name="salt"></param>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>The created credential view model</returns>
    public async Task<CredentialVm> CreateCredentialAsync(CredentialDto credentialDto, byte[] salt, string key, CancellationToken cancellationToken)
    {
        var credential = new Credential(
            credentialDto.Uri,
            credentialDto.Username.EncryptData(key, salt),
            credentialDto.Password.EncryptData(key, salt),
            credentialDto.Key?.EncryptData(key, salt),
            credentialDto.Key2?.EncryptData(key, salt),
            Convert.ToBase64String(salt),
            credentialDto.OperatorId);

        await context.Credentials.AddAsync(credential, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CredentialVm(
            credential.CredentialId,
            credential.Uri,
            credential.Username,
            credential.Password,
            credential.Key,
            credential.Key2);
    }

    /// <summary>
    /// Updates an existing credential asynchronously.
    /// </summary>
    /// <param name="credentialDto"></param>
    /// <param name="salt"></param>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateCredentialAsync(UpdateCredentialDto credentialDto, byte[] salt, string key, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialDto.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialDto.CredentialId}");

        context.Credentials.Attach(credential);

        credential.Uri = credentialDto.Uri;
        credential.Username = credentialDto.Username.EncryptData(key, salt);
        credential.Password = credentialDto.Password.EncryptData(key, salt);
        credential.Key = credentialDto.Key.EncryptData(key, salt);
        credential.Key2 = credentialDto.Key2.EncryptData(key, salt);
        credential.Salt = Convert.ToBase64String(salt);
        credential.Token = null;
        credential.TokenExpiration = null;
        credential.RefreshToken = null;
        credential.RefreshTokenExpiration = null;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Updates the token of an existing credential asynchronously.
    /// </summary>
    /// <param name="credentialDto"></param>
    /// <param name="key"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task UpdateTokenAsync(UpdateTokenDto credentialDto, string key, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialDto.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialDto.CredentialId}");

        context.Credentials.Attach(credential);

        var salt = Convert.FromBase64String(credential.Salt);
        credential.Token = credentialDto.Token?.EncryptData(key, salt);
        credential.TokenExpiration = credentialDto.TokenExpiration;
        credential.RefreshToken = credentialDto.RefreshToken?.EncryptData(key, salt);
        credential.RefreshTokenExpiration = credentialDto.RefreshTokenExpiration;

        await context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a credential asynchronously.
    /// </summary>
    /// <param name="credentialId"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="NotFoundException"></exception>
    public async Task DeleteCredentialAsync(Guid credentialId, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialId}");

        context.Credentials.Attach(credential);

        context.Credentials.Remove(credential);
        await context.SaveChangesAsync(cancellationToken);
    }
}
