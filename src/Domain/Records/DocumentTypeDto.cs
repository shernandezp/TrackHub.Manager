namespace TrackHub.Manager.Domain.Records;

// Account-configurable document type (Category) with required/expiring flags (spec 04 §7.1).
public readonly record struct DocumentTypeDto(
    Guid AccountId,
    string Category,
    string? DisplayName,
    bool Required,
    bool Expiring,
    int? DefaultValidityDays = null);
