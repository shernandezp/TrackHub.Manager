using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;

// This class represents a reader for the Transporter entity.
public sealed class TransporterReader(IApplicationDbContext context) : ITransporterReader
{
    // Retrieves a Transporter entity by its ID.
    // Parameters:
    //   id: The ID of the Transporter entity.
    //   cancellationToken: A cancellation token to cancel the operation.
    // Returns:
    //   A Task that represents the asynchronous operation. The task result contains the TransporterVm object.
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
