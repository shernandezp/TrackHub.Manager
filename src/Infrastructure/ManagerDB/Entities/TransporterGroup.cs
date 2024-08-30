namespace TrackHub.Manager.Infrastructure.ManagerDB.Entities;
public sealed class TransporterGroup
{
    private Transporter? _transporter;
    private Group? _group;

    public required Guid TransporterId { get; set; }
    public required long GroupId { get; set; }

    public Transporter Transporter
    {
        get => _transporter ?? throw new InvalidOperationException("Transporter is not loaded");
        set => _transporter = value;
    }
    public Group Group
    {
        get => _group ?? throw new InvalidOperationException("Group is not loaded");
        set => _group = value;
    }
}
