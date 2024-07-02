namespace TrackHub.Manager.Domain.Models;

public readonly record struct DeviceVm(
    Guid DeviceId,
    int Identifier,
    string Serial,
    string Name,
    DeviceType DeviceTypeId,
    string? Description
    );
