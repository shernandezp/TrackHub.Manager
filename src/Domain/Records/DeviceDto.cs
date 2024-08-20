namespace TrackHub.Manager.Domain.Records;
public readonly record struct DeviceDto(
    string Name,
    short DeviceTypeId,
    string? Description
    );
