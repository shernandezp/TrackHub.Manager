using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class DocumentWriter(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IDocumentWriter
{
    public async Task<DocumentVm> CreateDocumentMetadataAsync(DocumentDto document, CancellationToken cancellationToken)
    {
        var entity = new Document(RequireAccountAccess(document.AccountId), document.OwnerEntityType, document.OwnerEntityId, document.UploadedByPrincipalType, document.UploadedByPrincipalId, document.StorageProvider, document.StorageKey, document.ContentType, document.SizeBytes, document.Sha256Hash, document.Classification, document.Status, document.ExpiresAt, document.VisibilityScope, document.ScanStatus);
        await Context.Documents.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return ToVm(entity);
    }

    public async Task MarkDocumentUploadedAsync(Guid documentId, string status, CancellationToken cancellationToken) 
        => await UpdateDocumentAsync(documentId, x => x.Status = status, cancellationToken);

    public async Task MarkDocumentScanResultAsync(Guid documentId, string scanStatus, CancellationToken cancellationToken) 
        => await UpdateDocumentAsync(documentId, x => x.ScanStatus = scanStatus, cancellationToken);

    public async Task ExpireDocumentAsync(Guid documentId, DateTimeOffset expiresAt, CancellationToken cancellationToken) 
        => await UpdateDocumentAsync(documentId, x => { x.ExpiresAt = expiresAt; x.Status = "Expired"; }, cancellationToken);

    public async Task DeleteDocumentReferenceAsync(Guid documentId, CancellationToken cancellationToken) 
        => await UpdateDocumentAsync(documentId, x => x.Status = "Deleted", cancellationToken);

    private async Task UpdateDocumentAsync(Guid documentId, Action<Document> update, CancellationToken cancellationToken)
    {
        var entity = await Context.Documents.FirstAsync(x => x.DocumentId == documentId, cancellationToken);
        RequireAccountAccess(entity.AccountId);
        Context.Documents.Attach(entity);
        update(entity);
        await Context.SaveChangesAsync(cancellationToken);
    }

    private static DocumentVm ToVm(Document x) 
        => new(x.DocumentId, x.AccountId, x.OwnerEntityType, x.OwnerEntityId, x.UploadedByPrincipalType, x.UploadedByPrincipalId, x.StorageProvider, x.ContentType, x.SizeBytes, x.Sha256Hash, x.Classification, x.Status, x.ExpiresAt, x.VisibilityScope, x.ScanStatus, x.LastModified);
}
