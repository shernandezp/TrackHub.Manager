namespace TrackHub.Manager.Domain.Records;

public readonly record struct AccountSettingsDto(
    Guid AccountId,
    string Maps,
    string? MapsKey,
    int OnlineTimeLapse,
    bool StoreLastPosition,
    int StoringTimeLapse,
    bool RefreshMap,
    int RefreshMapTimer
    );
