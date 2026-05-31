using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;
using TrackHub.Manager.Domain.Enums;

namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransporterDeviceAssignmentReader
{
    Task<TransporterDeviceAssignmentVm> GetAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetByTransporterAsync(Guid transporterId, bool activeOnly, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetByDeviceAsync(Guid deviceId, bool activeOnly, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterDeviceAssignmentVm>> GetByAccountAsync(Guid accountId, bool activeOnly, CancellationToken cancellationToken);
}
