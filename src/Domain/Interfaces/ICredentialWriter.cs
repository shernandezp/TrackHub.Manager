using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface ICredentialWriter
{
    Task<CredentialVm> CreateCredentialAsync(CredentialDto credentialDto, string salt, CancellationToken cancellationToken);
    Task DeleteCredentialAsync(Guid credentialId, CancellationToken cancellationToken);
    Task UpdateCredentialAsync(UpdateCredentialDto credentialDto, byte[] key, CancellationToken cancellationToken);
    Task UpdateCredentialTokenAsync(UpdateCredentialTokenDto credentialDto, byte[] key, CancellationToken cancellationToken);
}
