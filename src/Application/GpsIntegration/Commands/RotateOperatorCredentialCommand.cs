using Ardalis.GuardClauses;
using Common.Application.Interfaces;
using Common.Domain.Extensions;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.Credentials, Action = Actions.Write)]
public readonly record struct RotateOperatorCredentialCommand(RotateCredentialDto Credential) : IRequest;

public class RotateOperatorCredentialCommandHandler(ICredentialWriter writer, IConfiguration configuration, ICurrentPrincipal principal)
    : IRequestHandler<RotateOperatorCredentialCommand>
{
    public async Task Handle(RotateOperatorCredentialCommand request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");

        var salt = CryptographyExtensions.GenerateAesKey(256);
        var actorId = principal.UserId?.ToString() ?? principal.ClientId ?? principal.SubjectId ?? "unknown";
        await writer.RotateCredentialAsync(request.Credential, salt, key, principal.PrincipalType.ToString(), actorId, cancellationToken);
    }
}
