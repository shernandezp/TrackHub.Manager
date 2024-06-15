namespace TrackHub.Manager.Infrastructure.Entities;

using Common.Infrastructure;

public sealed class Transporter(string name, short transporterTypeId, short icon, Guid deviceId) : BaseAuditableEntity
{
    private Device? _device;

    public Guid TransporterId { get; private set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public short TransporterTypeId { get; set; } = transporterTypeId;
    public short Icon { get; set; } = icon;
    public Guid DeviceId { get; set; } = deviceId;

    public Device Device
    {
        get => _device ?? throw new InvalidOperationException("Device is not loaded");
        set => _device = value;
    }
}
