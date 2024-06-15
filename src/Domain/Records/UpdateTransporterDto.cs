namespace TrackHub.Manager.Domain.Records;
public record struct UpdateTransporterDto(
    Guid TransporterId,
    string Name,
    TransporterType TransporterTypeId,
    short Icon,
    Guid DeviceId
    );
