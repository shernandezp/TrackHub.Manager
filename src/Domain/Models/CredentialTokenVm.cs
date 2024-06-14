namespace TrackHub.Manager.Domain.Models;
public readonly record struct CredentialTokenVm(
    string? Token,
    DateTime? TokenExpiration,
    string? RefreshToken,
    DateTime? RefreshTokenExpiration);
