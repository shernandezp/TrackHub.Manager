using Common.Infrastructure;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;
public sealed class Transporter(string name, short transporterTypeId) : BaseAuditableEntity
{
    public Guid TransporterId { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public short TransporterTypeId { get; set; } = transporterTypeId;

    public ICollection<Group> Groups { get; set; } = [];
    public ICollection<Device> Devices { get; set; } = [];
    public TransporterPosition? Position { get; set; }
}
