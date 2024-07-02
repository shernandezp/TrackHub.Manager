namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class DeviceOperator
{
    private Device? _device;
    private Operator? _operator;

    public required Guid DeviceId { get; set; }
    public required Guid OperatorId { get; set; }

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
