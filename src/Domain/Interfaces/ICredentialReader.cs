namespace TrackHub.Manager.Domain.Interfaces;
public interface ICredentialReader
{
    Task<CredentialVm> GetCredentialAsync(Guid id, byte[] key, CancellationToken cancellationToken);
    Task<CredentialTokenVm> GetCredentialTokenAsync(Guid id, byte[] key, CancellationToken cancellationToken);
}
