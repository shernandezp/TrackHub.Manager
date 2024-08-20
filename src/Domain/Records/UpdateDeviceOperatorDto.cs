namespace TrackHub.Manager.Domain.Records;
public record struct UpdateDeviceOperatorDto(
    long DeviceOperatorId,
    int Identifier,
    string Serial,
    Guid DeviceId, 
    Guid OperatorId
    );
