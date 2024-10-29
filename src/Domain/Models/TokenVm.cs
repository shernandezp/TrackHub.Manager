namespace TrackHub.Manager.Domain.Models;
public readonly record struct TokenVm(
    string? Token,
    DateTimeOffset? TokenExpiration,
    string? RefreshToken,
    DateTimeOffset? RefreshTokenExpiration);
