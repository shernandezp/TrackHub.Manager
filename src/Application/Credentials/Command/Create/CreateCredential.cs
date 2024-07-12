using Ardalis.GuardClauses;
using Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.Create;

[Authorize(Resource = Resources.Credentials, Action = Actions.Write)]
public readonly record struct CreateCredentialCommand(CredentialDto Credential) : IRequest<CredentialVm>;

// This class handles the logic for creating a credential
public class CreateCredentialCommandHandler(ICredentialWriter writer, IConfiguration configuration) : IRequestHandler<CreateCredentialCommand, CredentialVm>
{
    // This method handles the request to create a credential
    public async Task<CredentialVm> Handle(CreateCredentialCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        var salt = CryptographyExtensions.GenerateAesKey(256);
        return await writer.CreateCredentialAsync(request.Credential, salt, key, cancellationToken);
    }
}
