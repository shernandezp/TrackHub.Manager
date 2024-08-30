namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class UserSettings(Guid userId, string language, string style)
{
    public Guid UserId { get; set; } = userId;
    public string Language { get; set; } = language;
    public string Style { get; set; } = style;
}
