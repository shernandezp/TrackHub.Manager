namespace TrackHub.Manager.Application.Operators.Queries.GetByAccount;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorByAccountQuery(Guid AccountId) : IRequest<IReadOnlyCollection<OperatorVm>>;

public class GetOperatorsQueryHandler(IOperatorReader reader) : IRequestHandler<GetOperatorByAccountQuery, IReadOnlyCollection<OperatorVm>>
{
    public async Task<IReadOnlyCollection<OperatorVm>> Handle(GetOperatorByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorsByAccountAsync(request.AccountId, cancellationToken);

}
