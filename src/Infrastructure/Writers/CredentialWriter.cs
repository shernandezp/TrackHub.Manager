using Common.Domain.Extensions;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class CredentialWriter(IApplicationDbContext context) : ICredentialWriter
{
    public async Task<CredentialVm> CreateCredentialAsync(CredentialDto credentialDto, string salt, CancellationToken cancellationToken)
    {
        var credential = new Credential(
            credentialDto.Uri,
            credentialDto.Username,
            credentialDto.Password,
            credentialDto.Key,
            credentialDto.Key2,
            salt,
            credentialDto.OperatorId);

        await context.Credentials.AddAsync(credential, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new CredentialVm(
            credential.CredentialId,
            credential.Uri,
            credential.Username,
            credential.Password,
            credential.Key,
            credential.Key2,
            credential.OperatorId);
    }

    public async Task UpdateCredentialAsync(UpdateCredentialDto credentialDto, byte[] key, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialDto.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialDto.CredentialId}");

        credential.Uri = credentialDto.Uri;
        credential.Username = credentialDto.Username.EncryptStringToBase64_Aes(key);
        credential.Password = credentialDto.Password.EncryptStringToBase64_Aes(key);
        credential.Key = credentialDto.Key.EncryptStringToBase64_Aes(key);
        credential.Key2 = credentialDto.Key2.EncryptStringToBase64_Aes(key);
        credential.Salt = Convert.ToBase64String(key);
        credential.Token = null;
        credential.TokenExpiration = null;
        credential.RefreshToken = null;
        credential.RefreshTokenExpiration = null;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCredentialTokenAsync(UpdateCredentialTokenDto credentialDto, byte[] key, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialDto.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialDto.CredentialId}");

        credential.Token = credentialDto.Token?.EncryptStringToBase64_Aes(key);
        credential.TokenExpiration = credentialDto.TokenExpiration;
        credential.RefreshToken = credentialDto.RefreshToken?.EncryptStringToBase64_Aes(key);
        credential.RefreshTokenExpiration = credentialDto.RefreshTokenExpiration;

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCredentialAsync(Guid credentialId, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialId}");

        context.Credentials.Remove(credential);
        await context.SaveChangesAsync(cancellationToken);
    }
}
