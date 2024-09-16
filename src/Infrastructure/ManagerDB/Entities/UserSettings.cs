namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class UserSettings(Guid userId)
{
    private User? _user;

    public string Language { get; set; } = string.Empty;
    public string Style { get; set; } = "light";
    public Guid UserId { get; set; } = userId;

    public User User
    {
        get => _user ?? throw new InvalidOperationException("User is not loaded");
        set => _user = value;
    }
}
