namespace TrackHub.Manager.Domain.Records;

public readonly record struct RotateCredentialDto(
    Guid OperatorId,
    string Uri,
    string Username,
    string Password,
    string? Key,
    string? Key2);
