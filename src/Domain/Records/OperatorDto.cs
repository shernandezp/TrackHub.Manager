namespace TrackHub.Manager.Domain.Records;
public readonly record struct OperatorDto(
    string Name,
    string? Description,
    string? PhoneNumber,
    string? EmailAddress,
    string? Address,
    string? ContactName,
    short ProtocolTypeId,
    Guid AccountId,
    CredentialDto Credential
    );
