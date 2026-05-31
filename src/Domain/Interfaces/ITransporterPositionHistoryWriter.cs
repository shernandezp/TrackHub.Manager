using TrackHub.Manager.Domain.Records;

namespace TrackHub.Manager.Domain.Interfaces;

public interface ITransporterPositionHistoryWriter
{
    Task<bool> AppendAsync(TransporterPositionHistoryDto dto, CancellationToken cancellationToken);
    Task<int> PurgeOlderThanAsync(Guid accountId, DateTimeOffset cutoff, CancellationToken cancellationToken);
}
