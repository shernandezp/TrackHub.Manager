using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface IOperatorHealthCheckWriter
{
    Task<OperatorHealthCheckVm> RecordAsync(OperatorHealthCheckDto dto, CancellationToken cancellationToken);
}
