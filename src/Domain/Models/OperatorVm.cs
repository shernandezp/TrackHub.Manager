namespace TrackHub.Manager.Domain.Models;
public readonly record struct OperatorVm(
    Guid OperatorId,
    string Name,
    string? Description,
    string? PhoneNumber,
    string? EmailAddress,
    string? Address,
    string? ContactName,
    int ProtocolType
    );
