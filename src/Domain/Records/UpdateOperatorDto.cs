namespace TrackHub.Manager.Domain.Records;

public readonly record struct UpdateOperatorDto(
    Guid OperatorId,
    string Name,
    string? Description,
    string? PhoneNumber,
    string? EmailAddress,
    string? Address,
    string? ContactName,
    short ProtocolTypeId
    );
