using Common.Domain.Extensions;

namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class CredentialReader(IApplicationDbContext context) : ICredentialReader
{
    public async Task<CredentialVm> GetCredentialAsync(Guid id, byte[] key, CancellationToken cancellationToken)
        => await context.Credentials
            .Where(c => c.CredentialId.Equals(id))
            .Select(c => new CredentialVm(
                c.CredentialId,
                c.Uri,
                c.Username.DecryptStringFromBase64_Aes(key),
                c.Password.DecryptStringFromBase64_Aes(key),
                c.Key.DecryptStringFromBase64_Aes(key),
                c.Key2.DecryptStringFromBase64_Aes(key)))
            .FirstAsync(cancellationToken);

    public async Task<TokenVm> GetTokenAsync(Guid id, byte[] key, CancellationToken cancellationToken)
        => await context.Credentials
            .Where(c => c.CredentialId.Equals(id))
            .Select(c => new TokenVm(
                c.Token != null ? c.Token.DecryptStringFromBase64_Aes(key) : null,
                c.TokenExpiration,
                c.RefreshToken != null ? c.RefreshToken.DecryptStringFromBase64_Aes(key) : null,
                c.RefreshTokenExpiration))
            .FirstAsync(cancellationToken);

}
