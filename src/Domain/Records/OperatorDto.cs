namespace TrackHub.Manager.Domain.Records;
public readonly record struct OperatorDto(
    string Name,
    string? Description,
    string? PhoneNumber,
    string? EmailAddress,
    string? Address,
    string? ContactName,
    ProtocolType ProtocolType,
    Guid AccountId,
    CredentialDto Credential
    );
