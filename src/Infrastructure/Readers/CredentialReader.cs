using Common.Domain.Extensions;

namespace TrackHub.Manager.Infrastructure.Readers;

public sealed class CredentialReader(IApplicationDbContext context) : ICredentialReader
{
    public async Task<CredentialVm> GetCredentialAsync(Guid id, string key, CancellationToken cancellationToken)
        => await context.Credentials
            .Where(c => c.CredentialId.Equals(id))
            .Select(c => new CredentialVm(
                c.CredentialId,
                c.Uri,
                c.Username.DecryptData(key, Convert.FromBase64String(c.Salt)),
                c.Password.DecryptData(key, Convert.FromBase64String(c.Salt)),
                c.Key != null ? c.Key.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.Key2 != null ? c.Key2.DecryptData(key, Convert.FromBase64String(c.Salt)) : null))
            .FirstAsync(cancellationToken);


    //This method might not be needed
    public async Task<TokenVm> GetTokenAsync(Guid id, string key, CancellationToken cancellationToken)
        => await context.Credentials
            .Where(c => c.CredentialId.Equals(id))
            .Select(c => new TokenVm(
                c.Token != null ? c.Token.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.TokenExpiration,
                c.RefreshToken != null ? c.RefreshToken.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.RefreshTokenExpiration))
            .FirstAsync(cancellationToken);

}
