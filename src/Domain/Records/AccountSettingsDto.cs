namespace TrackHub.Manager.Domain.Records;

public readonly record struct AccountSettingsDto(
    Guid AccountId,
    string Maps,
    string? MapsKey,
    int OnlineInterval,
    bool StoreLastPosition,
    int StoringInterval,
    bool RefreshMap,
    int RefreshMapInterval
    );
