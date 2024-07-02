namespace TrackHub.Manager.Domain.Records;
public readonly record struct UpdateTokenDto(
    Guid CredentialId,
    string? Token,
    DateTime? TokenExpiration,
    string? RefreshToken,
    DateTime? RefreshTokenExpiration
    );
