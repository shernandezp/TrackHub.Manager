using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IPositionRetentionPolicyReader
{
    Task<PositionRetentionPolicyVm> GetAsync(Guid accountId, CancellationToken cancellationToken);
}
