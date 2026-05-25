using Common.Application.Interfaces;
using Common.Domain.Helpers;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class TransporterPositionHistoryReader(IApplicationDbContext context, ICurrentPrincipal principal)
    : AccountScopedDataAccess(context, principal), ITransporterPositionHistoryReader
{
    public async Task<IReadOnlyCollection<TransporterPositionHistoryVm>> GetAsync(Filters filters, int take, CancellationToken cancellationToken)
    {
        var pageSize = Math.Clamp(take <= 0 ? 100 : take, 1, 1000);
        var q = Context.TransporterPositionHistory.AsQueryable();
        q = filters.Apply(q);
        if (!CanAccessAllAccounts && Principal.AccountId.HasValue)
        {
            var acct = Principal.AccountId.Value;
            q = q.Where(x => x.AccountId == acct);
        }
        return await q.OrderByDescending(x => x.SourceTimestamp)
            .Take(pageSize)
            .Select(x => new TransporterPositionHistoryVm(x.TransporterPositionHistoryId, x.AccountId, x.OperatorId, x.DeviceId, x.TransporterId,
                x.SourceTimestamp, x.ReceivedAt, x.Latitude, x.Longitude, x.Altitude, x.Speed, x.Course, x.EventId,
                x.Address, x.City, x.State, x.Country, x.Attributes, x.IdempotencyKey))
            .ToListAsync(cancellationToken);
    }
}
