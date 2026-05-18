namespace TrackHub.Manager.Domain.Interfaces;

public interface IDocumentReader
{
    Task<IReadOnlyCollection<DocumentVm>> GetDocumentsForOwnerAsync(Guid accountId, string ownerEntityType, string ownerEntityId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken);
}
