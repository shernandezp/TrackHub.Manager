using TrackHub.Manager.Application.Accounts.Commands.Create;
using TrackHub.Manager.Application.Accounts.Commands.Update;
using TrackHub.Manager.Application.Accounts.Commands.UpdateSettings;

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

    public async Task<bool> UpdateAccountSettings([Service] ISender sender, Guid id, UpdateAccountSettingsCommand command)
    {
        if (id != command.AccountSettings.AccountId) return false;
        await sender.Send(command);
        return true;
    }
}
