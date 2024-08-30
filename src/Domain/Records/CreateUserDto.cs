namespace TrackHub.Manager.Domain.Records;

public record struct CreateUserDto(
    Guid AccountId,
    string Username,
    string Password,
    string EmailAddress,
    string FirstName,
    string LastName,
    bool Active);
