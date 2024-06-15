using Common.Domain.Enums;

namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class OperatorReader(IApplicationDbContext context) : IOperatorReader
{
    public async Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken)
        => await context.Operators
            .Where(o => o.OperatorId.Equals(id))
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                (ProtocolType)o.ProtocolType))
            .FirstAsync(cancellationToken);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Operators
            .Where(o => o.AccountId == accountId)
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                (ProtocolType)o.ProtocolType))
            .ToListAsync(cancellationToken);
}
