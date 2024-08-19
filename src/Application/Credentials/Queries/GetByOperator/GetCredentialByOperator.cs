using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Queries.GetByOperator;

[Authorize(Resource = Resources.Credentials, Action = Actions.Read)]
public readonly record struct GetCredentialByOperatorQuery(Guid OperatorId) : IRequest<CredentialVm>;

public class GetCredentialsByOperatorQueryHandler(ICredentialReader reader, IConfiguration configuration) : IRequestHandler<GetCredentialByOperatorQuery, CredentialVm>
{
    public async Task<CredentialVm> Handle(GetCredentialByOperatorQuery request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        return await reader.GetCredentialByOperatorAsync(request.OperatorId, key, cancellationToken);
    }
}
