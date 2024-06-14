namespace TrackHub.Manager.Domain.Records;
public readonly record struct CredentialDto(
    string Uri,
    string Username,
    string Password,
    string Key,
    string Key2,
    Guid OperatorId
    );
