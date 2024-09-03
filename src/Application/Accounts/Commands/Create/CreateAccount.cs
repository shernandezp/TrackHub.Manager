﻿namespace TrackHub.Manager.Application.Accounts.Commands.Create;

[Authorize(Resource = Resources.ManageAccounts, Action = Actions.Write)]
public readonly record struct CreateAccountCommand(AccountDto Account) : IRequest<AccountVm>;

public class CreateAccountCommandHandler(IAccountWriter writer, ISecurityWriter securityWriter) : IRequestHandler<CreateAccountCommand, AccountVm>
{
    // This class handles the logic for creating an account
    public async Task<AccountVm> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    { 
        var account = await writer.CreateAccountAsync(request.Account, cancellationToken);
        await securityWriter.CreateUserAsync(new CreateUserDto
        (
            account.AccountId,
            "Manager",
            request.Account.Password,
            request.Account.EmailAddress,
            request.Account.FirstName,
            request.Account.LastName,
            request.Account.Active
        ), cancellationToken);

        return account;
    }
}
