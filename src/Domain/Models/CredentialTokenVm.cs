namespace TrackHub.Manager.Domain.Models;
public readonly record struct CredentialTokenVm(
    Guid CredentialId,
    string Uri,
    string Username,
    string Password,
    string Salt,
    string? Key,
    string? Key2,
    string? Token,
    DateTimeOffset? TokenExpiration,
    string? RefreshToken,
    DateTimeOffset? RefreshTokenExpiration);
