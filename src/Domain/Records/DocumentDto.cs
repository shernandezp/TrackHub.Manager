namespace TrackHub.Manager.Domain.Records;

public readonly record struct DocumentDto(Guid AccountId, string OwnerEntityType, string OwnerEntityId, string UploadedByPrincipalType, string UploadedByPrincipalId, string StorageProvider, string StorageKey, string ContentType, long SizeBytes, string Sha256Hash, string Classification, string Status, DateTimeOffset? ExpiresAt, string VisibilityScope, string ScanStatus);
