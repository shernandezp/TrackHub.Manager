using Common.Domain.Extensions;

namespace TrackHub.Manager.Infrastructure.Readers;

// Represents a reader for retrieving credentials
public sealed class CredentialReader(IApplicationDbContext context) : ICredentialReader
{
    // Retrieves a credential asynchronously
    // Parameters:
    //   id: The ID of the credential
    //   key: The encryption key
    //   cancellationToken: The cancellation token
    // Returns:
    //   A CredentialVm object representing the retrieved credential
    public async Task<CredentialVm> GetCredentialAsync(Guid id, string key, CancellationToken cancellationToken)
    {
        return await context.Credentials
            .Where(c => c.CredentialId.Equals(id))
            .Select(c => new CredentialVm(
                c.CredentialId,
                c.Uri,
                c.Username.DecryptData(key, Convert.FromBase64String(c.Salt)),
                c.Password.DecryptData(key, Convert.FromBase64String(c.Salt)),
                c.Key != null ? c.Key.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.Key2 != null ? c.Key2.DecryptData(key, Convert.FromBase64String(c.Salt)) : null))
            .FirstAsync(cancellationToken);
    }


    public async Task<CredentialVm> GetCredentialByOperatorAsync(Guid operatorId, string key, CancellationToken cancellationToken)
    {
        return await context.Credentials
            .Where(c => c.OperatorId.Equals(operatorId))
            .Select(c => new CredentialVm(
                c.CredentialId,
                c.Uri,
                c.Username.DecryptData(key, Convert.FromBase64String(c.Salt)),
                c.Password.DecryptData(key, Convert.FromBase64String(c.Salt)),
                c.Key != null ? c.Key.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.Key2 != null ? c.Key2.DecryptData(key, Convert.FromBase64String(c.Salt)) : null))
            .FirstAsync(cancellationToken);
    }

    // Retrieves a token asynchronously
    // Parameters:
    //   id: The ID of the credential
    //   key: The encryption key
    //   cancellationToken: The cancellation token
    // Returns:
    //   A TokenVm object representing the retrieved token
    // Remarks:
    //   This method might not be needed
    public async Task<TokenVm> GetTokenAsync(Guid id, string key, CancellationToken cancellationToken)
    {
        return await context.Credentials
            .Where(c => c.CredentialId.Equals(id))
            .Select(c => new TokenVm(
                c.Token != null ? c.Token.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.TokenExpiration,
                c.RefreshToken != null ? c.RefreshToken.DecryptData(key, Convert.FromBase64String(c.Salt)) : null,
                c.RefreshTokenExpiration))
            .FirstAsync(cancellationToken);
    }
}
