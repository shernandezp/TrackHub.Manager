using TrackHub.Manager.Application.Accounts.Commands.Create;
using TrackHub.Manager.Application.Accounts.Commands.Delete;
using TrackHub.Manager.Application.Accounts.Commands.Update;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<AccountVm> CreateAccount([Service] ISender sender, CreateAccountCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateAccount([Service] ISender sender, Guid id, UpdateAccountCommand command)
    {
        if (id != command.Account.AccountId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteAccount([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteAccountCommand(id));
        return id;
    }
}
