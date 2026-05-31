using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Writers;

public sealed class TransporterPositionHistoryWriter(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), ITransporterPositionHistoryWriter
{
    public async Task<bool> AppendAsync(TransporterPositionHistoryDto dto, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(dto.AccountId);
        var exists = await Context.TransporterPositionHistory
            .AnyAsync(x => x.AccountId == scoped && x.IdempotencyKey == dto.IdempotencyKey, cancellationToken);
        if (exists)
        {
            return false;
        }
        var entity = new TransporterPositionHistory(
            scoped, dto.OperatorId, dto.DeviceId, dto.TransporterId,
            dto.SourceTimestamp, DateTimeOffset.UtcNow,
            dto.Latitude, dto.Longitude, dto.Altitude, dto.Speed, dto.Course,
            dto.EventId, dto.Address, dto.City, dto.State, dto.Country, dto.Attributes, dto.IdempotencyKey);
        await Context.TransporterPositionHistory.AddAsync(entity, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<int> PurgeOlderThanAsync(Guid accountId, DateTimeOffset cutoff, CancellationToken cancellationToken)
    {
        var scoped = RequireAccountAccess(accountId);
        return await Context.TransporterPositionHistory
            .Where(x => x.AccountId == scoped && x.SourceTimestamp < cutoff)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
