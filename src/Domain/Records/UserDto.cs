namespace TrackHub.Manager.Domain.Records;
public record struct UserDto (
    Guid UserId,
    string Username,
    bool Active,
    Guid AccountId);
