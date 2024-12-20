﻿using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;

namespace TrackHub.Manager.Application.Credentials.Queries.Get;

[Authorize(Resource = Resources.Credentials, Action = Actions.Read)]
public readonly record struct GetCredentialQuery(Guid Id) : IRequest<CredentialVm>;

// Handles the GetCredentialQuery and returns the corresponding CredentialVm
public class GetCredentialsQueryHandler(ICredentialReader reader, IConfiguration configuration) : IRequestHandler<GetCredentialQuery, CredentialVm>
{
    // Handles the GetCredentialQuery and returns the corresponding CredentialVm
    public async Task<CredentialVm> Handle(GetCredentialQuery request, CancellationToken cancellationToken)
    {
        var key = configuration["AppSettings:EncryptionKey"];
        Guard.Against.Null(key, message: "Credential key not found.");
        return await reader.GetCredentialAsync(request.Id, key, cancellationToken);
    }
}
