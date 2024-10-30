using Common.Application.Extensions;
using Common.Application.GraphQL.Inputs;

namespace TrackHub.Manager.Application.Accounts.Queries.GetMaster;

[Authorize(Resource = Resources.AccountsMaster, Action = Actions.Read)]
public readonly record struct GetAccountSettingsMasterQuery(FiltersInput Filter) : IRequest<IReadOnlyCollection<AccountSettingsVm>>;

public class GetAccountSettingsMasterQueryHandler(IAccountSettingsReader reader) : IRequestHandler<GetAccountSettingsMasterQuery, IReadOnlyCollection<AccountSettingsVm>>
{
    public async Task<IReadOnlyCollection<AccountSettingsVm>> Handle(GetAccountSettingsMasterQuery request, CancellationToken cancellationToken)
        => await reader.GetAccountSettingsAsync(request.Filter.GetFilters(), cancellationToken);

}
