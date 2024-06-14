
namespace TrackHub.Manager.Domain.Records;
public readonly record struct UpdateDeviceDto(
    Guid DeviceId,
    string Identifier,
    string Name,
    DeviceType DeviceTypeId,
    string? Description
    );
