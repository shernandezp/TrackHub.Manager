namespace TrackHub.Manager.Domain.Interfaces;

public interface IReportReader
{
    Task<IReadOnlyCollection<ReportVm>> GetReportsAsync(CancellationToken cancellationToken);
}
