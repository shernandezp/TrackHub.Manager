namespace TrackHub.Manager.Domain.Models;
public record struct DeviceVm(
    Guid DeviceId,
    string Name,
    int Identifier,
    string Serial,
    short DeviceTypeId,
    string? Description,
    Guid TransporterId, 
    Guid OperatorId);
