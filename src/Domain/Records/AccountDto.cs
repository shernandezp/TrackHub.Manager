namespace TrackHub.Manager.Domain.Records;

public readonly record struct AccountDto(
    string Name,
    string? Description,
    short TypeId,
    bool Active,
    string Password,
    string EmailAddress,
    string FirstName,
    string LastName
    );
