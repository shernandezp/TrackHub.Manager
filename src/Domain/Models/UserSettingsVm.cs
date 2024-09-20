namespace TrackHub.Manager.Domain.Models;
public record struct UserSettingsVm(
    string Language,
    string Style,
    string Navbar,
    Guid UserId);
