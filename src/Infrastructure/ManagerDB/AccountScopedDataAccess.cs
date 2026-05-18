using Common.Application.Exceptions;
using Common.Application.Interfaces;
using Common.Domain.Constants;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB;

public abstract class AccountScopedDataAccess(IApplicationDbContext context, ICurrentPrincipal principal)
{
    protected IApplicationDbContext Context { get; } = context;
    protected ICurrentPrincipal Principal { get; } = principal;

    protected bool CanAccessAllAccounts => Principal.PrincipalType == PrincipalType.ServiceClient && !Principal.AccountId.HasValue;

    protected Guid RequireAccountAccess(Guid accountId)
    {
        if (accountId == Guid.Empty)
        {
            throw new ForbiddenAccessException();
        }

        if (CanAccessAllAccounts || Principal.AccountId == accountId || HasActiveSupportGrant(accountId))
        {
            return accountId;
        }

        throw new ForbiddenAccessException();
    }

    protected Guid? ResolveAccountScope(Guid? requestedAccountId)
    {
        if (CanAccessAllAccounts)
        {
            return requestedAccountId;
        }

        if (!Principal.AccountId.HasValue)
        {
            throw new ForbiddenAccessException();
        }

        if (requestedAccountId.HasValue && requestedAccountId.Value != Principal.AccountId.Value)
        {
            throw new ForbiddenAccessException();
        }

        return Principal.AccountId.Value;
    }

    protected void AddAuditEvent(Guid accountId, string action, string resourceType, string resourceId, string? oldValuesJson, string? newValuesJson)
    {
        var actorId = Principal.UserId?.ToString()
            ?? Principal.DriverId?.ToString()
            ?? Principal.ClientId
            ?? Principal.SubjectId
            ?? "unknown";

        Context.AuditEvents.Add(new AuditEvent(
            accountId,
            Principal.PrincipalType.ToString(),
            actorId,
            action,
            resourceType,
            resourceId,
            "Succeeded",
            oldValuesJson,
            newValuesJson,
            null,
            null,
            null,
            Principal.CorrelationId));
    }

    private bool HasActiveSupportGrant(Guid accountId)
    {
        if (!Principal.UserId.HasValue || !string.Equals(Principal.Role, Roles.Administrator, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        var now = DateTimeOffset.UtcNow;
        return Context.AccountSupportGrants.Any(x =>
            x.AccountId == accountId
            && x.SupportUserId == Principal.UserId.Value
            && x.ApprovedAt.HasValue
            && x.StartsAt <= now
            && x.EndsAt >= now
            && !x.RevokedAt.HasValue);
    }

    protected static string Quote(DateTimeOffset? value) => value.HasValue ? Quote(value.Value.ToString("O")) : "null";
    protected static string Quote(string? value) => value == null ? "null" : $"\"{value.Replace("\\", "\\\\").Replace("\"", "\\\"")}\"";
}
