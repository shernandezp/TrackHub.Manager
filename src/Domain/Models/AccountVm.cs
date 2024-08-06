
namespace TrackHub.Manager.Domain.Models;
public readonly record struct AccountVm(
    Guid AccountId,
    string Name,
    string? Description,
    AccountType Type,
    bool Active,
    DateTimeOffset LastModified
    );
