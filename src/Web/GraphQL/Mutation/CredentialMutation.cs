using TrackHub.Manager.Application.Credentials.Command.Create;
using TrackHub.Manager.Application.Credentials.Command.Delete;
using TrackHub.Manager.Application.Credentials.Command.Update;
using TrackHub.Manager.Application.Credentials.Command.UpdateToken;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<CredentialVm> CreateCredential([Service] ISender sender, CreateCredentialCommand command)
        => await sender.Send(command);

    public async Task<bool> UpdateCredential([Service] ISender sender, Guid id, UpdateCredentialCommand command)
    {
        if (id != command.Credential.CredentialId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<bool> UpdateCredentialToken([Service] ISender sender, Guid id, UpdateCredentialTokenCommand command)
    {
        if (id != command.Credential.CredentialId) return false;
        await sender.Send(command);
        return true;
    }

    public async Task<Guid> DeleteCredential([Service] ISender sender, Guid id)
    {
        await sender.Send(new DeleteCredentialCommand(id));
        return id;
    }
}
