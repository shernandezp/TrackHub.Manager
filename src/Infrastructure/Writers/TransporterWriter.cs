using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Writers;
public sealed class TransporterWriter(IApplicationDbContext context) : ITransporterWriter
{
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

    public async Task DeleteTransporterAsync(Guid transporterId, CancellationToken cancellationToken)
    {
        var transporter = await context.Transporters.FindAsync([transporterId], cancellationToken)
            ?? throw new NotFoundException(nameof(Transporter), $"{transporterId}");

        context.Transporters.Remove(transporter);
        await context.SaveChangesAsync(cancellationToken);
    }
}
