namespace TrackHub.Manager.Domain.Records;
public readonly record struct DeviceDto(
    string Identifier,
    string Name,
    DeviceType DeviceTypeId,
    string? Description
    );
