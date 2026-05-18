namespace TrackHub.Manager.Domain.Interfaces;

public interface IDocumentWriter
{
    Task<DocumentVm> CreateDocumentMetadataAsync(DocumentDto document, CancellationToken cancellationToken);
    Task MarkDocumentUploadedAsync(Guid documentId, string status, CancellationToken cancellationToken);
    Task MarkDocumentScanResultAsync(Guid documentId, string scanStatus, CancellationToken cancellationToken);
    Task ExpireDocumentAsync(Guid documentId, DateTimeOffset expiresAt, CancellationToken cancellationToken);
    Task DeleteDocumentReferenceAsync(Guid documentId, CancellationToken cancellationToken);
}
