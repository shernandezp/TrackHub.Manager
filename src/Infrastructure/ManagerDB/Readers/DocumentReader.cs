using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class DocumentReader(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IDocumentReader
{
    private static int PageSize(int take) => Math.Clamp(take <= 0 ? 50 : take, 1, 500);
    private static int Offset(int skip) => Math.Max(0, skip);

    public async Task<IReadOnlyCollection<DocumentVm>> GetDocumentsForOwnerAsync(Guid accountId, string ownerEntityType, string ownerEntityId, DateTimeOffset? from, DateTimeOffset? to, int skip, int take, CancellationToken cancellationToken)
    {
        var scopedAccountId = RequireAccountAccess(accountId);
        return await Context.Documents
            .Where(x => x.AccountId == scopedAccountId && x.OwnerEntityType == ownerEntityType && x.OwnerEntityId == ownerEntityId && (!from.HasValue || x.LastModified >= from) && (!to.HasValue || x.LastModified <= to))
            .OrderByDescending(x => x.LastModified).ThenBy(x => x.DocumentId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new DocumentVm(x.DocumentId, x.AccountId, x.OwnerEntityType, x.OwnerEntityId, x.UploadedByPrincipalType, x.UploadedByPrincipalId, x.StorageProvider, x.ContentType, x.SizeBytes, x.Sha256Hash, x.Classification, x.Status, x.ExpiresAt, x.VisibilityScope, x.ScanStatus, x.LastModified))
            .ToListAsync(cancellationToken);
    }
}
