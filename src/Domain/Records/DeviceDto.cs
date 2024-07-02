namespace TrackHub.Manager.Domain.Records;
public readonly record struct DeviceDto(
    int Identifier,
    string Serial,
    string Name,
    DeviceType DeviceTypeId,
    string? Description
    );
