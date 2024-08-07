using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Operators.Queries.GetByUser;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorByUserQuery() : IRequest<IReadOnlyCollection<OperatorVm>>;

public class GetOperatorsByUserQueryHandler(IOperatorReader reader, IUser user) : IRequestHandler<GetOperatorByUserQuery, IReadOnlyCollection<OperatorVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<OperatorVm>> Handle(GetOperatorByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetOperatorsByUserAsync(UserId, cancellationToken);

}
