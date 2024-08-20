namespace TrackHub.Manager.Domain.Records;

public readonly record struct UpdateDeviceDto(
    Guid DeviceId,
    string Name,
    short DeviceTypeId,
    string? Description
    );
