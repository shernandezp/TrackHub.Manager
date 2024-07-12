using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;

// This class represents a writer for the Transporter entity
public sealed class TransporterWriter(IApplicationDbContext context) : ITransporterWriter
{
    // Creates a new Transporter entity and saves it to the database
    public async Task<TransporterVm> CreateTransporterAsync(TransporterDto transporterDto, CancellationToken cancellationToken)
    {
        var transporter = new Transporter(
            transporterDto.Name,
            (short)transporterDto.TransporterTypeId,
            transporterDto.Icon,
            transporterDto.DeviceId);

        await context.Transporters.AddAsync(transporter, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new TransporterVm(
            transporter.TransporterId,
            transporter.Name,
            (TransporterType)transporter.TransporterTypeId,
            transporter.Icon,
            transporter.DeviceId);
    }

    // Updates an existing Transporter entity in the database
    public async Task UpdateTransporterAsync(UpdateTransporterDto transporterDto, CancellationToken cancellationToken)
    {
        var transporter = await context.Transporters.FindAsync([transporterDto.TransporterId], cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{transporterDto.TransporterId}");

        transporter.Name = transporterDto.Name;
        transporter.Icon = transporterDto.Icon;
        transporter.TransporterTypeId = (short)transporterDto.TransporterTypeId;
        transporter.DeviceId = transporterDto.DeviceId;

        await context.SaveChangesAsync(cancellationToken);
    }

    // Deletes a Transporter entity from the database
    public async Task DeleteTransporterAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        var transporter = await context.Transporters.FindAsync([transporterId], cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{transporterId}");

        context.Transporters.Remove(transporter);
        await context.SaveChangesAsync(cancellationToken);
    }
}
