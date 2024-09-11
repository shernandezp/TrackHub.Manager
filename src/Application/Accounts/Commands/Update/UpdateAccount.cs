namespace TrackHub.Manager.Application.Accounts.Commands.Update;

[Authorize(Resource = Resources.Administrative, Action = Actions.Edit)]
public readonly record struct UpdateAccountCommand(UpdateAccountDto Account) : IRequest;

public class UpdateAccountCommandHandler(IAccountWriter writer) : IRequestHandler<UpdateAccountCommand>
{
    // Handles the update account command
    public async Task Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        => await writer.UpdateAccountAsync(request.Account, cancellationToken);
}

