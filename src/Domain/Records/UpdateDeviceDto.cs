namespace TrackHub.Manager.Domain.Records;

public readonly record struct UpdateDeviceDto(
    Guid DeviceId,
    int Identifier,
    string Serial,
    string Name,
    DeviceType DeviceTypeId,
    string? Description
    );
