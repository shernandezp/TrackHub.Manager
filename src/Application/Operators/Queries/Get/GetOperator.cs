namespace TrackHub.Manager.Application.Operators.Queries.Get;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorQuery(Guid Id) : IRequest<OperatorVm>;

public class GetOperatorsQueryHandler(IOperatorReader reader) : IRequestHandler<GetOperatorQuery, OperatorVm>
{
    public async Task<OperatorVm> Handle(GetOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorAsync(request.Id, cancellationToken);

}
