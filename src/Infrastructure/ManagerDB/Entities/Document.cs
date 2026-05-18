using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class Document(Guid accountId, string ownerEntityType, string ownerEntityId, string uploadedByPrincipalType, string uploadedByPrincipalId, string storageProvider, string storageKey, string contentType, long sizeBytes, string sha256Hash, string classification, string status, DateTimeOffset? expiresAt, string visibilityScope, string scanStatus) : BaseAuditableEntity
{
    public Guid DocumentId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public string OwnerEntityType { get; set; } = ownerEntityType;
    public string OwnerEntityId { get; set; } = ownerEntityId;
    public string UploadedByPrincipalType { get; set; } = uploadedByPrincipalType;
    public string UploadedByPrincipalId { get; set; } = uploadedByPrincipalId;
    public string StorageProvider { get; set; } = storageProvider;
    public string StorageKey { get; set; } = storageKey;
    public string ContentType { get; set; } = contentType;
    public long SizeBytes { get; set; } = sizeBytes;
    public string Sha256Hash { get; set; } = sha256Hash;
    public string Classification { get; set; } = classification;
    public string Status { get; set; } = status;
    public DateTimeOffset? ExpiresAt { get; set; } = expiresAt;
    public string VisibilityScope { get; set; } = visibilityScope;
    public string ScanStatus { get; set; } = scanStatus;
}
