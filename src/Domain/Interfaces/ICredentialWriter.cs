using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface ICredentialWriter
{
    Task<CredentialVm> CreateCredentialAsync(CredentialDto credentialDto, byte[] key, CancellationToken cancellationToken);
    Task DeleteCredentialAsync(Guid credentialId, CancellationToken cancellationToken);
    Task UpdateCredentialAsync(UpdateCredentialDto credentialDto, byte[] key, CancellationToken cancellationToken);
    Task UpdateTokenAsync(UpdateTokenDto credentialDto, byte[] key, CancellationToken cancellationToken);
}
