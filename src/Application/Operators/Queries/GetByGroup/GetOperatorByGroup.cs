namespace TrackHub.Manager.Application.Operators.Queries.GetByGroup;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorByGroupQuery(long GroupId) : IRequest<IReadOnlyCollection<OperatorVm>>;

public class GetOperatorsQueryHandler(IOperatorReader reader) : IRequestHandler<GetOperatorByGroupQuery, IReadOnlyCollection<OperatorVm>>
{
    public async Task<IReadOnlyCollection<OperatorVm>> Handle(GetOperatorByGroupQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorsByGroupAsync(request.GroupId, cancellationToken);

}
