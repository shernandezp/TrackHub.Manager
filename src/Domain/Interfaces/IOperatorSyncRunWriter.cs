using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IOperatorSyncRunWriter
{
    Task<OperatorSyncRunVm> RecordAsync(OperatorSyncRunDto dto, CancellationToken cancellationToken);
}
