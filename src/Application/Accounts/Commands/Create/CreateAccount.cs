namespace TrackHub.Manager.Application.Accounts.Commands.Create;

[Authorize(Resource = Resources.Accounts, Action = Actions.Write)]
public readonly record struct CreateAccountCommand(AccountDto Account) : IRequest<AccountVm>;

public class CreateAccountCommandHandler(IAccountWriter writer) : IRequestHandler<CreateAccountCommand, AccountVm>
{
    public async Task<AccountVm> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        => await writer.CreateAccountAsync(request.Account, cancellationToken);
}
