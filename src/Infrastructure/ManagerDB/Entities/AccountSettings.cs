namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class AccountSettings (Guid accountId)
{
    private Account? _account;

    public Guid AccountId { get; set; } = accountId;
    public string Maps { get; set; } = "OSM";
    public string? MapsKey { get; set; } = "";
    public int OnlineTimeLapse { get; set; } = 60;
    public bool StoreLastPosition { get; set; } = false;
    public int StoringTimeLapse { get; set; } = 360;
    public bool RefreshMap { get; set; } = false;
    public int RefreshMapTimer { get; set; } = 60;

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
