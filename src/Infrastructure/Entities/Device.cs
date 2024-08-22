namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class Device (string name, int identifier, string serial, short deviceTypeId, string? description, Guid transporterId, Guid operatorId)
{
    private Transporter? _transporter;
    private Operator? _operator;
    public Guid DeviceId { get; private set; }
    public string Name { get; set; } = name;
    public int Identifier { get; set; } = identifier;
    public string Serial { get; set; } = serial;
    public short DeviceTypeId { get; set; } = deviceTypeId;
    public string? Description { get; set; } = description;
    public Guid TransporterId { get; set; } = transporterId;
    public Guid OperatorId { get; set; } = operatorId;

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }
    public Operator Operator
    {
        get => _operator ?? throw new InvalidOperationException("Operator is not loaded");
        set => _operator = value;
    }

}
