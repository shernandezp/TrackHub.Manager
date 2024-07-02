using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.Entities;
public sealed class Device(int identifier, string serial, string name, short deviceTypeId, string? description) : BaseAuditableEntity
{
    public Guid DeviceId { get; private set; } = Guid.NewGuid();
    public int Identifier { get; set; } = identifier;
    public string Serial { get; set; } = serial;
    public string Name { get; set; } = name;
    public short DeviceTypeId { get; set; } = deviceTypeId;
    public string? Description { get; set; } = description;
    public Transporter? Transporter { get; set; }
    public ICollection<Group> Groups { get; set; } = [];
    public ICollection<Operator> Operators { get; set; } = [];
}
