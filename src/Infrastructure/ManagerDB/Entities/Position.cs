namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;

public sealed class Position
{
    private Transporter? _transporter;

    public long PositionId { get; set; }
    public Guid TransporterId { get; set; }
    public Guid DeviceId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public DateTimeOffset DeviceDateTime { get; set; }
    public DateTimeOffset? ServerDateTime { get; set; }
    public double Speed { get; set; }
    public double? Course { get; set; }
    public int? EventId { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? Attributes { get; set; }

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }

}
