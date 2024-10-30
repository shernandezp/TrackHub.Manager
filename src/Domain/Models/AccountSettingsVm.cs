namespace TrackHub.Manager.Domain.Models;

public readonly record struct AccountSettingsVm(
    Guid AccountId,
    string Maps,
    string? MapsKey,
    int OnlineInterval,
    bool StoreLastPosition,
    int StoringInterval,
    bool RefreshMap,
    int RefreshMapInterval
    );
