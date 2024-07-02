using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.Update;

[Authorize(Resource = Resources.Credentials, Action = Actions.Edit)]
public readonly record struct UpdateCredentialCommand(UpdateCredentialDto Credential) : IRequest;

public class UpdateCredentialCommandHandler(ICredentialWriter writer, IConfiguration configuration) : IRequestHandler<UpdateCredentialCommand>
{
    public async Task Handle(UpdateCredentialCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        await writer.UpdateCredentialAsync(request.Credential, Convert.FromBase64String(key), cancellationToken); 
    }
}
