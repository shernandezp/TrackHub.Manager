using Common.Application.Interfaces;
using Common.Domain.Constants;
using Common.Domain.Helpers;
using Common.Domain.Enums;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class OperatorReader(
    IApplicationDbContext context,
    ICurrentPrincipal principal,
    IIdentityService identityService) : IOperatorReader
{
    private static OperatorVm Map(Entities.Operator o, bool includeCredential, DateTimeOffset? lastHealthCheckAt = null)
        => new(
            o.OperatorId,
            o.Name,
            o.Description,
            o.PhoneNumber,
            o.EmailAddress,
            o.Address,
            o.ContactName,
            (ProtocolType)o.ProtocolType,
            o.ProtocolType,
            o.AccountId,
            o.LastModified,
            includeCredential && o.Credential != null ? new CredentialTokenVm(
                o.Credential.CredentialId,
                o.Credential.Uri,
                o.Credential.Username,
                o.Credential.Password,
                o.Credential.Salt,
                o.Credential.Key,
                o.Credential.Key2,
                o.Credential.Token,
                o.Credential.TokenExpiration,
                o.Credential.RefreshToken,
                o.Credential.RefreshTokenExpiration) : null,
            o.Enabled,
            o.SyncIntervalMinutes,
            (OperatorHealthStatus)o.HealthStatus,
            o.LastSuccessfulSyncAt,
            o.LastFailedSyncAt,
            o.LastManualSyncAt,
            o.LastDeviceSyncAt,
            o.LastPositionSyncAt,
            o.LastFailureCode,
            o.LastFailureMessage,
            o.LastLatencyMs,
            lastHealthCheckAt);

    private async Task<DateTimeOffset?> GetLastHealthAsync(Guid operatorId, CancellationToken cancellationToken)
        => await context.OperatorHealthChecks
            .Where(c => c.OperatorId == operatorId)
            .OrderByDescending(c => c.StartedAt)
            .Select(c => (DateTimeOffset?)c.StartedAt)
            .FirstOrDefaultAsync(cancellationToken);

    private async Task<bool> CanIncludeCredentialsAsync(CancellationToken cancellationToken)
    {
        if (principal.PrincipalType == PrincipalType.ServiceClient)
        {
            return true;
        }

        if (principal.PrincipalType != PrincipalType.User || !principal.UserId.HasValue)
        {
            return false;
        }

        var userId = principal.UserId.Value;
        var roleAuthorized = await identityService.IsInRoleAsync(userId, Resources.Credentials, Actions.Custom, cancellationToken);
        return roleAuthorized
               && await identityService.AuthorizeAsync(userId, Resources.Credentials, Actions.Custom, cancellationToken);
    }

    private async Task<Dictionary<Guid, DateTimeOffset>> GetLastHealthByOperatorsAsync(IReadOnlyCollection<Guid> operatorIds, CancellationToken cancellationToken)
    {
        if (operatorIds.Count == 0) return [];
        return await context.OperatorHealthChecks
            .Where(c => operatorIds.Contains(c.OperatorId))
            .GroupBy(c => c.OperatorId)
            .Select(g => new { OperatorId = g.Key, Last = g.Max(x => x.StartedAt) })
            .ToDictionaryAsync(x => x.OperatorId, x => x.Last, cancellationToken);
    }

    public async Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken)
    {
        var op = await context.Operators
            .Include(o => o.Credential)
            .FirstOrDefaultAsync(o => o.OperatorId == id, cancellationToken)
            ?? throw new NotFoundException(nameof(Entities.Operator), id.ToString());
        var lastHealth = await GetLastHealthAsync(id, cancellationToken);
        var includeCredentials = await CanIncludeCredentialsAsync(cancellationToken);
        return Map(op, includeCredentials, lastHealth);
    }

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsAsync(Filters filters, CancellationToken cancellationToken)
    {
        var query = context.Operators.Include(o => o.Credential).AsQueryable();
        query = filters.Apply(query);
        var items = await query.ToListAsync(cancellationToken);
        var lastHealth = await GetLastHealthByOperatorsAsync(items.Select(o => o.OperatorId).ToList(), cancellationToken);
        var includeCredentials = await CanIncludeCredentialsAsync(cancellationToken);
        return items.Select(o => Map(o, includeCredentials, lastHealth.TryGetValue(o.OperatorId, out var t) ? t : null)).ToList();
    }

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var accountId = await context.Users
            .Where(u => u.UserId == userId)
            .Select(u => u.AccountId)
            .FirstAsync(cancellationToken);

        var items = await context.Operators
            .Include(o => o.Credential)
            .Where(o => o.AccountId == accountId)
            .ToListAsync(cancellationToken);
        var lastHealth = await GetLastHealthByOperatorsAsync(items.Select(o => o.OperatorId).ToList(), cancellationToken);
        var includeCredentials = await CanIncludeCredentialsAsync(cancellationToken);
        return items.Select(o => Map(o, includeCredentials, lastHealth.TryGetValue(o.OperatorId, out var t) ? t : null)).ToList();
    }
}
