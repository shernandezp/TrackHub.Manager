
namespace TrackHub.Manager.Domain.Models;
public readonly record struct AccountSettingsVm(
    Guid AccountId,
    string Maps,
    string? MapsKey,
    int OnlineTimeLapse,
    bool StoreLastPosition,
    int StoringTimeLapse,
    bool RefreshMap,
    int RefreshMapTimer
    );
