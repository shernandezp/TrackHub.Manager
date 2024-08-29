using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Operators.Queries.GetByAccount;

[Authorize(Resource = Resources.Operators, Action = Actions.Read)]
public readonly record struct GetOperatorByCurrentAccountQuery() : IRequest<IReadOnlyCollection<OperatorVm>>;

public class GetOperatorsCurrentAccountQueryHandler(IOperatorReader reader, IUserReader userReader, IUser user) : IRequestHandler<GetOperatorByCurrentAccountQuery, IReadOnlyCollection<OperatorVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<OperatorVm>> Handle(GetOperatorByCurrentAccountQuery request, CancellationToken cancellationToken)
    {
        // Get the account associated with the current user
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        // Get the operators associated with the account
        return await reader.GetOperatorsByAccountAsync(user.AccountId, cancellationToken);
    }

}
