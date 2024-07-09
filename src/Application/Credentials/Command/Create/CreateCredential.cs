using Ardalis.GuardClauses;
using Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.Create;

[Authorize(Resource = Resources.Credentials, Action = Actions.Write)]
public readonly record struct CreateCredentialCommand(CredentialDto Credential) : IRequest<CredentialVm>;

public class CreateCredentialCommandHandler(ICredentialWriter writer, IConfiguration configuration) : IRequestHandler<CreateCredentialCommand, CredentialVm>
{
    public async Task<CredentialVm> Handle(CreateCredentialCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        var salt = CryptographyExtensions.GenerateAesKey(256);
        return await writer.CreateCredentialAsync(request.Credential, salt, key, cancellationToken);
    }
}
