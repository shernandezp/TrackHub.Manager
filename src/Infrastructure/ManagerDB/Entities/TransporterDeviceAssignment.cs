namespace TrackHub.Manager.Infrastructure.Entities;

public sealed class TransporterDeviceAssignment(
    Guid accountId,
    Guid transporterId,
    Guid deviceId,
    DateTimeOffset effectiveFrom,
    int priority,
    bool isPrimary,
    int status,
    string? assignmentReason,
    string createdByPrincipalType,
    string createdByPrincipalId)
{
    private Transporter? _transporter;
    private Device? _device;

    public Guid TransporterDeviceAssignmentId { get; private set; } = Guid.NewGuid();
    public Guid AccountId { get; set; } = accountId;
    public Guid TransporterId { get; set; } = transporterId;
    public Guid DeviceId { get; set; } = deviceId;
    public DateTimeOffset EffectiveFrom { get; set; } = effectiveFrom;
    public DateTimeOffset? EffectiveTo { get; set; }
    public int Priority { get; set; } = priority;
    public bool IsPrimary { get; set; } = isPrimary;
    public int Status { get; set; } = status;
    public string? AssignmentReason { get; set; } = assignmentReason;
    public string CreatedByPrincipalType { get; set; } = createdByPrincipalType;
    public string CreatedByPrincipalId { get; set; } = createdByPrincipalId;

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }

    public Device Device
    {
        get => _device ?? throw new InvalidOperationException("Device is not loaded");
        set => _device = value;
    }
}
