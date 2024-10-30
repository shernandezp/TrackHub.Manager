using Common.Application.Extensions;
using Common.Application.GraphQL.Inputs;

namespace TrackHub.Manager.Application.Operators.Queries.GetMaster;

[Authorize(Resource = Resources.OperatorsMaster, Action = Actions.Read)]
public readonly record struct GetOperatorMasterQuery(FiltersInput Filter) : IRequest<IReadOnlyCollection<OperatorVm>>;

public class GetOperatorsMasterQueryHandler(IOperatorReader reader) : IRequestHandler<GetOperatorMasterQuery, IReadOnlyCollection<OperatorVm>>
{
    public async Task<IReadOnlyCollection<OperatorVm>> Handle(GetOperatorMasterQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorsAsync(request.Filter.GetFilters(), cancellationToken);

}
