using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;
// This class represents a writer for the Transporter entity
public sealed class TransporterWriter(IApplicationDbContext context) : ITransporterWriter
{
    // Creates a new transporter asynchronously
    // Parameters:
    // - transporterDto: The transporter data transfer object
    // - cancellationToken: The cancellation token
    // Returns:
    // - The created transporter view model
    public async Task<TransporterVm> CreateTransporterAsync(TransporterDto transporterDto, CancellationToken cancellationToken)
    {
        var transporter = new Transporter(
            transporterDto.Name,
            transporterDto.TransporterTypeId);

        await context.Transporters.AddAsync(transporter, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TransporterVm(
            transporter.TransporterId,
            transporter.Name,
            (TransporterType)transporter.TransporterTypeId,
            transporter.TransporterTypeId);
    }

    // Updates an existing transporter asynchronously
    // Parameters:
    // - transporterDto: The updated transporter data transfer object
    // - cancellationToken: The cancellation token
    public async Task UpdateTransporterAsync(UpdateTransporterDto transporterDto, CancellationToken cancellationToken)
    {
        var transporter = await context.Transporters.FindAsync(transporterDto.TransporterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{transporterDto.TransporterId}");

        context.Transporters.Attach(transporter);

        transporter.Name = transporterDto.Name;
        transporter.TransporterTypeId = transporterDto.TransporterTypeId;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes a transporter asynchronously
    // Parameters:
    // - transporterId: The ID of the transporter to delete
    // - cancellationToken: The cancellation token
    public async Task DeleteTransporterAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        var transporter = await context.Transporters.FindAsync(transporterId, cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{transporterId}");

        context.Transporters.Attach(transporter);

        context.Transporters.Remove(transporter);
        await context.SaveChangesAsync(cancellationToken);
    }

}
