using TrackHub.Manager.Application.Credentials.Queries.Get;
using TrackHub.Manager.Application.Credentials.Queries.GetByOperator;
using TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<CredentialVm> GetCredential([Service] ISender sender, [AsParameters] GetCredentialQuery query)
        => await sender.Send(query);

    public async Task<CredentialVm> GetCredentialByOperator([Service] ISender sender, [AsParameters] GetCredentialByOperatorQuery query)
        => await sender.Send(query);

    public async Task<TokenVm> GetToken([Service] ISender sender, [AsParameters] GetTokenQuery query)
        => await sender.Send(query);
}
