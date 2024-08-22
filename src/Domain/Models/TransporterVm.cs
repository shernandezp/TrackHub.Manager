namespace TrackHub.Manager.Domain.Models;

public readonly record struct TransporterVm(
    Guid TransporterId,
    string Name,
    TransporterType TransporterType,
    short TransporterTypeId,
    short Icon
    );
