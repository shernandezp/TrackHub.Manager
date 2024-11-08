﻿namespace TrackHub.Manager.Domain.Records;

public record struct TransporterPositionDto(
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
    AttributesDto? Attributes
    );
