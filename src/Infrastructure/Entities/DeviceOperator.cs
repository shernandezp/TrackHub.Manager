namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class DeviceOperator (int identifier, string serial, Guid deviceId, Guid operatorId)
{
    private Device? _device;
    private Operator? _operator;
    public long DeviceOperatorId { get; private set; }
    public int Identifier { get; set; } = identifier;
    public string Serial { get; set; } = serial;
    public Guid DeviceId { get; set; } = deviceId;
    public Guid OperatorId { get; set; } = operatorId;

    public Device Device
    {
        get => _device ?? throw new InvalidOperationException("Device is not loaded");
        set => _device = value;
    }
    public Operator Operator
    {
        get => _operator ?? throw new InvalidOperationException("Operator is not loaded");
        set => _operator = value;
    }

}
