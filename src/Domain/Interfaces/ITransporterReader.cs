namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransporterReader
{
    Task<TransporterVm> GetTransporterAsync(Guid id, CancellationToken cancellationToken);
    Task<TransporterVm> GetTransporterAsync(string name, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterVm>> GetTransportersByAccountAsync(Guid accountId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterVm>> GetTransportersByGroupAsync(long groupId, CancellationToken cancellationToken);
    Task<IReadOnlyCollection<TransporterVm>> GetTransportersByUserAsync(Guid userId, CancellationToken cancellationToken);
}
