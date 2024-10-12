namespace TrackHub.Manager.Domain.Models;
public record struct DeviceTransporterVm(
    Guid DeviceId,
    int Identifier,
    string Serial,
    string Name,
    TransporterType TransporterType,
    short TransporterTypeId);
