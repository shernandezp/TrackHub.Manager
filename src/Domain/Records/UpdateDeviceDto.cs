namespace TrackHub.Manager.Domain.Records;
public record struct UpdateDeviceDto(
    Guid DeviceId,
    string Name,
    int Identifier,
    short DeviceTypeId,
    string? Description,
    Guid TransporterId
    );
