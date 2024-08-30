namespace TrackHub.Manager.Domain.Records;
public readonly record struct UpdateAccountDto(
    Guid AccountId,
    string Name,
    string? Description,
    short Type,
    bool Active
    );
