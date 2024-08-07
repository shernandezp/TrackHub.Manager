namespace TrackHub.Manager.Domain.Records;

public readonly record struct UpdateOperatorCredentialDto(
    Guid OperatorId,
    string Uri,
    string Username,
    string Password,
    string Key,
    string Key2
    );
