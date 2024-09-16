namespace TrackHub.Manager.Domain.Records;

public readonly record struct AccountSettingsDto(
    Guid AccountId,
    string Maps,
    bool StoreLastPosition
    );
