namespace TrackHub.Manager.Domain.Records;
public record struct UpdateUserDto(
    Guid UserId,
    string Username,
    bool Active);
