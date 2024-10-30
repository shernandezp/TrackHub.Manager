using TrackHub.Manager.Application.Operators.Queries.Get;
using TrackHub.Manager.Application.Operators.Queries.GetByAccount;
using TrackHub.Manager.Application.Operators.Queries.GetMaster;
using TrackHub.Manager.Application.Operators.Queries.GetByUser;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<OperatorVm> GetOperator([Service] ISender sender, [AsParameters] GetOperatorQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsMaster([Service] ISender sender, [AsParameters] GetOperatorMasterQuery query)
        => await sender.Send(query);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByCurrentAccount([Service] ISender sender)
        => await sender.Send(new GetOperatorByCurrentAccountQuery());

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUser([Service] ISender sender)
        => await sender.Send(new GetOperatorByUserQuery());

}
