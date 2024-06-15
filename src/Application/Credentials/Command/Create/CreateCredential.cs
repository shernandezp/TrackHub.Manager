using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.Create;

[Authorize(Resource = Resources.Accounts, Action = Actions.Write)]
public readonly record struct CreateCredentialCommand(CredentialDto Credential) : IRequest<CredentialVm>;

public class CreateCredentialCommandHandler(ICredentialWriter writer, IConfiguration configuration) : IRequestHandler<CreateCredentialCommand, CredentialVm>
{
    public async Task<CredentialVm> Handle(CreateCredentialCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        return await writer.CreateCredentialAsync(request.Credential, Convert.FromBase64String(key), cancellationToken);
    }
}
