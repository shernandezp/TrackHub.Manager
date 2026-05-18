using TrackHub.Manager.Application.AccountSupportGrants.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<AccountSupportGrantVm> CreateAccountSupportGrant([Service] ISender sender, CreateAccountSupportGrantCommand command) => await sender.Send(command);
    public async Task<bool> ApproveAccountSupportGrant([Service] ISender sender, ApproveAccountSupportGrantCommand command) { await sender.Send(command); return true; }
    public async Task<bool> RevokeAccountSupportGrant([Service] ISender sender, RevokeAccountSupportGrantCommand command) { await sender.Send(command); return true; }
}
