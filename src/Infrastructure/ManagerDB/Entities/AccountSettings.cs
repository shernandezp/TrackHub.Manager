namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class AccountSettings (Guid accountId)
{
    private Account? _account;

    public Guid AccountId { get; set; } = accountId;
    public string Maps { get; set; } = "OSM";
    public bool StoreLastPosition { get; set; } = false;
    //Background service running each x minutes on the server side to update the position of the devices
    //Fallback method in the router to get the last known position from the db

    public Account Account
    {
        get => _account ?? throw new InvalidOperationException("Account is not loaded");
        set => _account = value;
    }
}
