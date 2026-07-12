namespace TrackHub.Manager.Domain.Records;

// Evidence-only signature capture (spec 04 §6.3) — not a legal e-signature workflow.
public readonly record struct DocumentSignatureDto(
    Guid DocumentId,
    string SignerPrincipalType,
    string SignerPrincipalId,
    string SignerName,
    string LegalTextAccepted,
    Guid? SignatureImageDocumentId = null,
    double? Latitude = null,
    double? Longitude = null);
