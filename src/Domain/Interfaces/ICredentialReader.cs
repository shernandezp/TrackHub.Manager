﻿namespace TrackHub.Manager.Domain.Interfaces;
public interface ICredentialReader
{
    Task<CredentialVm> GetCredentialAsync(Guid id, byte[] key, CancellationToken cancellationToken);
    Task<TokenVm> GetTokenAsync(Guid id, byte[] key, CancellationToken cancellationToken);
}