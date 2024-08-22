namespace TrackHub.Manager.Domain.Records;
public record struct DeviceDto(
    string Name,
    int Identifier,
    string Serial,
    short DeviceTypeId,
    string? Description,
    Guid TransporterId, 
    Guid OperatorId
    );
