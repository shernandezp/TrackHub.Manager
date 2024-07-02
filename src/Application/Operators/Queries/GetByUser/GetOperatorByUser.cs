namespace TrackHub.Manager.Application.Operators.Queries.GetByUser;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorByUserQuery(Guid UserId) : IRequest<IReadOnlyCollection<OperatorVm>>;

public class GetOperatorsQueryHandler(IOperatorReader reader) : IRequestHandler<GetOperatorByUserQuery, IReadOnlyCollection<OperatorVm>>
{
    public async Task<IReadOnlyCollection<OperatorVm>> Handle(GetOperatorByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorsByUserAsync(request.UserId, cancellationToken);

}
