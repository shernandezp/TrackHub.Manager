namespace TrackHub.Manager.Domain.Records;
public record struct UserSettingsDto(
    string Language,
    string Style,
    string Navbar,
    Guid UserId);
