namespace TrackHub.Manager.Infrastructure.Writers;

// TransporterGroupWriter class for writing transporter group data
public sealed class TransporterGroupWriter(IApplicationDbContext context) : ITransporterGroupWriter
{
    // Create a new transporter group asynchronously
    // Parameters:
    // - transporterGroupDto: The transporter group data transfer object
    // - cancellationToken: The cancellation token
    // Returns:
    // - The created transporter group view model
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
    // Parameters:
    // - transporterId: The ID of the transporter
    // - groupId: The ID of the group
    // - cancellationToken: The cancellation token
    public async Task DeleteTransporterGroupAsync(Guid transporterId, long groupId, CancellationToken cancellationToken)
    {
        var transporterGroup = await context.TransportersGroup.FindAsync([transporterId, groupId], cancellationToken);

        if (transporterGroup != default)
        {
            context.TransportersGroup.Attach(transporterGroup);

            context.TransportersGroup.Remove(transporterGroup);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    // Delete all transporter groups that match the given transporterId asynchronously
    // Parameters:
    // - transporterId: The ID of the transporter
    // - cancellationToken: The cancellation token
    public async Task DeleteTransporterGroupsAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        var transporterGroups = await context.TransportersGroup
            .Where(tg => tg.TransporterId == transporterId)
            .ToListAsync(cancellationToken);

        if (transporterGroups.Count != 0)
        {
            context.TransportersGroup.AttachRange(transporterGroups);

            context.TransportersGroup.RemoveRange(transporterGroups);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
