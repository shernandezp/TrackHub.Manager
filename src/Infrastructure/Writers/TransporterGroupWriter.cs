namespace TrackHub.Manager.Infrastructure.Writers;

// TransporterGroupWriter class for writing transporter group data
public sealed class TransporterGroupWriter(IApplicationDbContext context) : ITransporterGroupWriter
{
    // Create a new transporter group asynchronously
    public async Task<TransporterGroupVm> CreateTransporterGroupAsync(TransporterGroupDto transporterGroupDto, CancellationToken cancellationToken)
    {
        var transporterGroup = new TransporterGroup
        {
            TransporterId = transporterGroupDto.TransporterId,
            GroupId = transporterGroupDto.GroupId
        };

        await context.TransportersGroup.AddAsync(transporterGroup, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TransporterGroupVm(
            transporterGroup.TransporterId,
            transporterGroup.GroupId);
    }

    // Delete a transporter group asynchronously
    public async Task DeleteTransporterGroupAsync(Guid transporterId, long groupId, CancellationToken cancellationToken)
    {
        var transporterGroup = await context.TransportersGroup.FindAsync([transporterId, groupId], cancellationToken)
            ?? throw new NotFoundException(nameof(TransporterGroup), $"{transporterId},{groupId}");

        context.TransportersGroup.Attach(transporterGroup);

        context.TransportersGroup.Remove(transporterGroup);
        await context.SaveChangesAsync(cancellationToken);
    }
}
