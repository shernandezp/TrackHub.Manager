using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IPositionRetentionPolicyWriter
{
    Task SetAsync(Guid accountId, PositionRetentionPolicyDto dto, CancellationToken cancellationToken);
}
