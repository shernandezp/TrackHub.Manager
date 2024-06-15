using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class TransporterReader(IApplicationDbContext context) : ITransporterReader
{
    public async Task<TransporterVm> GetTransporterAsync(Guid id, CancellationToken cancellationToken)
        => await context.Transporters
            .Where(a => a.TransporterId.Equals(id))
            .Select(a => new TransporterVm(
                a.TransporterId,
                a.Name,
                (TransporterType)a.TransporterTypeId,
                a.Icon,
                a.DeviceId))
            .FirstAsync(cancellationToken);

}
