using Common.Application.Exceptions;
using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class GroupVisibilityReader(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), IGroupVisibilityReader
{
    public async Task<bool> ValidateGroupVisibilityAsync(Guid accountId, Guid userId, string resourceType, string resourceId, CancellationToken cancellationToken)
    {
        var scopedAccountId = RequireAccountAccess(accountId);

        if (!CanAccessAllAccounts && Principal.UserId.HasValue && Principal.UserId.Value != userId)
        {
            throw new ForbiddenAccessException();
        }

        if (!Guid.TryParse(resourceId, out var parsedResourceId) || !string.Equals(resourceType, "Transporter", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        return await Context.Users
            .Where(user => user.UserId == userId && user.AccountId == scopedAccountId && user.Active)
            .SelectMany(user => user.Groups.Select(group => group.GroupId))
            .Intersect(Context.Transporters
                .Where(transporter => transporter.TransporterId == parsedResourceId && transporter.AccountId == scopedAccountId)
                .SelectMany(transporter => transporter.Groups.Select(group => group.GroupId)))
            .AnyAsync(cancellationToken);
    }
}
