namespace TrackHub.Manager.Domain.Records;
public readonly record struct UpdateCredentialDto(
    Guid CredentialId,
    string Uri,
    string Username,
    string Password,
    string Key,
    string Key2
    );
