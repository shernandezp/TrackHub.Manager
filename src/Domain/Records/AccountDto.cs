namespace TrackHub.Manager.Domain.Records;

public readonly record struct AccountDto(
    string Name,
    string? Description,
    AccountType Type,
    bool Active,
    string Password,
    string EmailAddress,
    string FirstName,
    string LastName
    );
