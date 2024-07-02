using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.UpdateToken;

[Authorize(Resource = Resources.Credentials, Action = Actions.Edit)]
public readonly record struct UpdateTokenCommand(UpdateTokenDto Credential) : IRequest;

public class UpdateCommandHandler(ICredentialWriter writer, IConfiguration configuration) : IRequestHandler<UpdateTokenCommand>
{
    public async Task Handle(UpdateTokenCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        await writer.UpdateTokenAsync(request.Credential, Convert.FromBase64String(key), cancellationToken);
    }
}
