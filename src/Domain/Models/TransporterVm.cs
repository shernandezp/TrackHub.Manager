namespace TrackHub.Manager.Domain.Models;
public record struct TransporterVm(
    Guid TransporterId,
    string Name,
    TransporterType TransporterTypeId,
    short Icon,
    Guid? DeviceId
    );
