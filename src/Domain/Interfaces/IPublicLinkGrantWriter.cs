namespace TrackHub.Manager.Domain.Interfaces;

public interface IPublicLinkGrantWriter
{
    Task<PublicLinkGrantVm> CreatePublicLinkGrantAsync(PublicLinkGrantDto publicLinkGrant, CancellationToken cancellationToken);
    Task RevokePublicLinkGrantAsync(Guid publicLinkGrantId, string revokedBy, CancellationToken cancellationToken);
    Task RecordPublicLinkAccessAsync(Guid publicLinkGrantId, CancellationToken cancellationToken);
}
