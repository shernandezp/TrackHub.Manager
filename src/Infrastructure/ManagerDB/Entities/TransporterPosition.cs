namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public class TransporterPosition(
    Guid transporterId,
    Guid? geometryId,
    double latitude,
    double longitude,
    double? altitude,
    DateTime dateTime,
    TimeSpan offset,
    double speed,
    double? course,
    int? eventId,
    string? address,
    string? city,
    string? state,
    string? country,
    AttributesVm? attributes
    )
{
    private Transporter? _transporter;

    public Guid TransporterPositionId { get; set; }
    public Guid TransporterId { get; set; } = transporterId;
    public Guid? GeometryId { get; set; } = geometryId;
    public double Latitude { get; set; } = latitude;
    public double Longitude { get; set; } = longitude;
    public double? Altitude { get; set; } = altitude;
    public DateTime DateTime { get; set; } = dateTime;
    public TimeSpan Offset { get; set; } = offset;
    public double Speed { get; set; } = speed;
    public double? Course { get; set; } = course;
    public int? EventId { get; set; } = eventId;
    public string? Address { get; set; } = address;
    public string? City { get; set; } = city;
    public string? State { get; set; } = state;
    public string? Country { get; set; } = country;
    public AttributesVm? Attributes { get; set; } = attributes;

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }

}
