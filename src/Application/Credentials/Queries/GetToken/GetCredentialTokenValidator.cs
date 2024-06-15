namespace TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

public class GetCredentialTokenValidator : AbstractValidator<GetCredentialTokenQuery>
{
    public GetCredentialTokenValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
