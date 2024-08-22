namespace TrackHub.Manager.Domain.Records;
public record struct UpdateDeviceDto(
    Guid DeviceId,
    string Name,
    int Identifier,
    string Serial,
    short DeviceTypeId,
    string? Description,
    Guid TransporterId, 
    Guid OperatorId
    );
