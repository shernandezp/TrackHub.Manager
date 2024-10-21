﻿namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public sealed class TransporterPosition(
    Guid transporterId,
    Guid? geometryId,
    double latitude,
    double longitude,
    double? altitude,
    DateTimeOffset deviceDateTime,
    double speed,
    double? course,
    int? eventId,
    string? address,
    string? city,
    string? state,
    string? country,
    string? attributes
    )
{
    private Transporter? _transporter;

    public Guid TransporterPositionId { get; set; } = Guid.NewGuid();
    public Guid TransporterId { get; set; } = transporterId;
    public Guid? GeometryId { get; set; } = geometryId;
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
    public double? Altitude { get; set; } = altitude;
    public DateTimeOffset DeviceDateTime { get; set; } = deviceDateTime;
    public double Speed { get; set; } = speed;
    public double? Course { get; set; } = course;
    public int? EventId { get; set; } = eventId;
    public string? Address { get; set; } = address;
    public string? City { get; set; } = city;
    public string? State { get; set; } = state;
    public string? Country { get; set; } = country;
    public string? Attributes { get; set; } = attributes;

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }

}
