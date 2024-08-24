namespace TrackHub.Manager.Domain.Records;

public readonly record struct UpdateTransporterDto(
    Guid TransporterId,
    string Name,
    short TransporterTypeId
    );
