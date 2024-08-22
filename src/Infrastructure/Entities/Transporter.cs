using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.Entities;
public sealed class Transporter(string name, short transporterTypeId, short icon) : BaseAuditableEntity
{
    public Guid TransporterId { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public short TransporterTypeId { get; set; } = transporterTypeId;
    public short Icon { get; set; } = icon;

    public ICollection<Group> Groups { get; set; } = [];
    public ICollection<Device> Devices { get; set; } = [];
}
