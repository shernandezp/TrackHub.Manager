namespace TrackHub.Manager.Infrastructure.Entities;

public class AccountSettings
{
    public Guid AccountId { get; set; }
    public bool StoreLastPosition { get; set; }
    //Background service running each x minutes on the server side to update the position of the devices
    //Fallback method in the router to get the last known position from the db
}
