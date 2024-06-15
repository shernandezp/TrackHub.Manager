using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;
public interface IOperatorWriter
{
    Task<OperatorVm> CreateOperatorAsync(OperatorDto operatorDto, CancellationToken cancellationToken);
    Task DeleteOperatorAsync(Guid operatorId, CancellationToken cancellationToken);
    Task UpdateOperatorAsync(UpdateOperatorDto operatorDto, CancellationToken cancellationToken);
}
