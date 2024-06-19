using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.UpdateToken;

[Authorize(Resource = Resources.Accounts, Action = Actions.Edit)]
public readonly record struct UpdateCredentialTokenCommand(UpdateCredentialTokenDto Credential) : IRequest;

public class UpdateCredentialCommandHandler(ICredentialWriter writer, IConfiguration configuration) : IRequestHandler<UpdateCredentialTokenCommand>
{
    public async Task Handle(UpdateCredentialTokenCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        await writer.UpdateCredentialTokenAsync(request.Credential, Convert.FromBase64String(key), cancellationToken);
    }
}
