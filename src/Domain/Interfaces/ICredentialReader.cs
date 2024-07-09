namespace TrackHub.Manager.Domain.Interfaces;
public interface ICredentialReader
{
    Task<CredentialVm> GetCredentialAsync(Guid id, string key, CancellationToken cancellationToken);
    Task<TokenVm> GetTokenAsync(Guid id, string key, CancellationToken cancellationToken);
}
