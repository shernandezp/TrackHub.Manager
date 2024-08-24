namespace TrackHub.Manager.Domain.Records;

public record struct ProcessDeviceDto(
    string Name,
    int Identifier,
    string Serial,
    short DeviceTypeId,
    short TransporterTypeId,
    string? Description
    );
