using Common.Application.Interfaces;
using TrackHub.Manager.Infrastructure.Interfaces;

namespace TrackHub.Manager.Infrastructure.ManagerDB.Readers;

public sealed class NotificationReader(IApplicationDbContext context, ICurrentPrincipal principal) : AccountScopedDataAccess(context, principal), INotificationReader
{
    private static int PageSize(int take) => Math.Clamp(take <= 0 ? 50 : take, 1, 500);
    private static int Offset(int skip) => Math.Max(0, skip);

    public async Task<IReadOnlyCollection<NotificationRuleVm>> GetNotificationRulesAsync(Guid accountId, int skip, int take, CancellationToken cancellationToken)
    {
        var scopedAccountId = RequireAccountAccess(accountId);
        return await Context.NotificationRules
            .Where(x => x.AccountId == scopedAccountId)
            .OrderBy(x => x.RuleKey).ThenBy(x => x.NotificationRuleId)
            .Skip(Offset(skip)).Take(PageSize(take))
            .Select(x => new NotificationRuleVm(x.NotificationRuleId, x.AccountId, x.RuleKey, x.RuleType, x.Enabled, x.TriggerEvent, x.RecipientSelector, x.ChannelsJson, x.ThrottlingJson, x.ConfigurationJson, x.LastModified))
            .ToListAsync(cancellationToken);
    }
}
