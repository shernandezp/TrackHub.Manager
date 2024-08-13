using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Operators.Commands.Create;

[Authorize(Resource = Resources.Operators, Action = Actions.Write)]
public readonly record struct CreateOperatorCommand(OperatorDto Operator) : IRequest<OperatorVm>;

public class CreateOperatorCommandHandler(IOperatorWriter writer, IUserReader userReader, IUser user) : IRequestHandler<CreateOperatorCommand, OperatorVm>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    // This method handles the CreateOperatorCommand and creates an operator.
    public async Task<OperatorVm> Handle(CreateOperatorCommand request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await writer.CreateOperatorAsync(request.Operator, user.AccountId, cancellationToken);
    }
}
