namespace TrackHub.Manager.Domain.Models;
public record struct DeviceOperatorVm(
    long DeviceOperatorId,
    string Name,
    int Identifier,
    string Serial,
    Guid DeviceId, 
    Guid OperatorId);
