using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

[Authorize(Resource = Resources.Credentials, Action = Actions.Read)]
public readonly record struct GetTokenQuery(Guid Id) : IRequest<TokenVm>;

public class GetTokenQueryHandler(ICredentialReader reader, IConfiguration configuration) : IRequestHandler<GetTokenQuery, TokenVm>
{
    public async Task<TokenVm> Handle(GetTokenQuery request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "CredentialToken key not found.");
        return await reader.GetTokenAsync(request.Id, key, cancellationToken);
    }

}
