namespace TrackHub.Manager.Domain.Models;

public record struct TransporterPositionVm(
    Guid TransporterPositionId,
    Guid TransporterId,
    Guid? GeometryId,
    double Latitude,
    double Longitude,
    double? Altitude,
    DateTimeOffset DeviceDateTime,
    double Speed,
    double? Course,
    int? EventId,
    string? Address,
    string? City,
    string? State,
    string? Country,
    string? Attributes);
