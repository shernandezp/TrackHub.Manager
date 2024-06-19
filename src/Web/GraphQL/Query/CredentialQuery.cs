using TrackHub.Manager.Application.Credentials.Queries.Get;
using TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<CredentialVm> GetCredential([Service] ISender sender, [AsParameters] GetCredentialQuery query)
        => await sender.Send(query);

    public async Task<CredentialTokenVm> GetCredentialTokenAccount([Service] ISender sender, [AsParameters] GetCredentialTokenQuery query)
        => await sender.Send(query);
}
