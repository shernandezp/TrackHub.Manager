
namespace TrackHub.Manager.Domain.Models;
public readonly record struct AccountSettingsVm(
    Guid AccountId,
    string Maps,
    bool StoreLastPosition
    );
