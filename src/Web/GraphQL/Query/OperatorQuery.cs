using TrackHub.Manager.Application.Operators.Queries.Get;
using TrackHub.Manager.Application.Operators.Queries.GetByAccount;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<OperatorVm> GetOperator([Service] ISender sender, [AsParameters] GetOperatorQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByAccount([Service] ISender sender, [AsParameters] GetOperatorByAccountQuery query)
        => await sender.Send(query);
}
