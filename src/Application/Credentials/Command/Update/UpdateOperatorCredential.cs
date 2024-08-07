using Ardalis.GuardClauses;
using Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Command.Update;

[Authorize(Resource = Resources.Credentials, Action = Actions.Edit)]
public readonly record struct UpdateOperatorCredentialCommand(UpdateOperatorCredentialDto Credential) : IRequest;

public class UpdateOperatorCredentialCommandHandler(ICredentialWriter writer, IOperatorReader operatorReader, IConfiguration configuration) : IRequestHandler<UpdateOperatorCredentialCommand>
{
    public async Task Handle(UpdateOperatorCredentialCommand request, CancellationToken cancellationToken)
    {
        // Retrieve the encryption key from the configuration
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");

        // Generate a salt for encryption
        var salt = CryptographyExtensions.GenerateAesKey(256);

        // Retrieve the operator based on the provided OperatorId
        var @operator = await operatorReader.GetOperatorAsync(request.Credential.OperatorId, cancellationToken);
        Guard.Against.Null(@operator.Credential, nameof(@operator.Credential));
        var credentialId = @operator.Credential.Value.CredentialId;

        // Update the credential using the writer
        await writer.UpdateCredentialAsync(
            new UpdateCredentialDto(
                credentialId,
                request.Credential.Uri,
                request.Credential.Username,
                request.Credential.Password,
                request.Credential.Key,
                request.Credential.Key2
            ), salt, key, cancellationToken);
    }
}
