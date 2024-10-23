namespace TrackHub.Manager.Domain.Models;

public record struct DeviceTransporterVm(
    Guid TransporterId,
    int Identifier,
    string Serial,
    string Name,
    TransporterType TransporterType,
    short TransporterTypeId);
