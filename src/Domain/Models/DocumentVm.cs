namespace TrackHub.Manager.Domain.Models;

public readonly record struct DocumentVm(Guid DocumentId, Guid AccountId, string OwnerEntityType, string OwnerEntityId, string UploadedByPrincipalType, string UploadedByPrincipalId, string StorageProvider, string ContentType, long SizeBytes, string Sha256Hash, string Classification, string Status, DateTimeOffset? ExpiresAt, string VisibilityScope, string ScanStatus, DateTimeOffset LastModified);
