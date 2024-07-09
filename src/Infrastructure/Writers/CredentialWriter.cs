using Common.Domain.Extensions;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class CredentialWriter(IApplicationDbContext context) : ICredentialWriter
{
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

    public async Task UpdateCredentialAsync(UpdateCredentialDto credentialDto, byte[] salt, string key, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialDto.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialDto.CredentialId}");

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

    public async Task UpdateTokenAsync(UpdateTokenDto credentialDto, string key, CancellationToken cancellationToken)
    {
        var credential = await context.Credentials.FindAsync([credentialDto.CredentialId], cancellationToken)
            ?? throw new NotFoundException(nameof(Credential), $"{credentialDto.CredentialId}");

        var salt = Convert.FromBase64String(credential.Salt);
        credential.Token = credentialDto.Token?.EncryptData(key, salt);
        credential.TokenExpiration = credentialDto.TokenExpiration;
        credential.RefreshToken = credentialDto.RefreshToken?.EncryptData(key, salt);
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
