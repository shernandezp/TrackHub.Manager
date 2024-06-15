using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

[Authorize(Resource = Resources.Accounts, Action = Actions.Read)]
public readonly record struct GetCredentialTokenQuery(Guid Id) : IRequest<CredentialTokenVm>;

public class GetCredentialTokenQueryHandler(ICredentialReader reader, IConfiguration configuration) : IRequestHandler<GetCredentialTokenQuery, CredentialTokenVm>
{
    public async Task<CredentialTokenVm> Handle(GetCredentialTokenQuery request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "CredentialToken key not found.");
        return await reader.GetCredentialTokenAsync(request.Id, Convert.FromBase64String(key), cancellationToken);
    }

}
