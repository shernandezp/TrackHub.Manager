namespace TrackHub.Manager.Domain.Models;
public readonly record struct DeviceVm(
    Guid DeviceId,
    string Identifier,
    string Name,
    short DeviceTypeId,
    string? Description
    );
