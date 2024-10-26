using Common.Application.GraphQL.Inputs;
using TrackHub.Manager.Domain;

namespace TrackHub.Manager.Application.Accounts.Queries.GetAll;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetAccountsSettingsQuery(FiltersInput Filter) : IRequest<IReadOnlyCollection<AccountSettingsVm>>;

public class GetAccountSettingsQueryHandler(IAccountSettingsReader reader) : IRequestHandler<GetAccountsSettingsQuery, IReadOnlyCollection<AccountSettingsVm>>
{
    public async Task<IReadOnlyCollection<AccountSettingsVm>> Handle(GetAccountsSettingsQuery request, CancellationToken cancellationToken)
    {
        var filtersDictionary = request.Filter.Filters.ToDictionary(f => f.Key, f => f.Value);
        var filters = new Filters(filtersDictionary);
        return await reader.GetAccountSettingsAsync(filters, cancellationToken); 
    }
}
