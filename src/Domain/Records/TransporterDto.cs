namespace TrackHub.Manager.Domain.Records;
public record struct TransporterDto(
    string Name,
    TransporterType TransporterTypeId,
    short Icon,
    Guid DeviceId
    );
