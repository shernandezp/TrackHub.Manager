namespace TrackHub.Manager.Domain.Models;

public readonly record struct DeviceVm(
    Guid DeviceId,
    string Name,
    DeviceType DeviceType,
    short DeviceTypeId,
    string? Description
    );
