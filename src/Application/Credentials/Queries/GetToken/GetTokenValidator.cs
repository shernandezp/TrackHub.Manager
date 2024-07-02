namespace TrackHub.Manager.Application.CredentialToken.Queries.GetToken;

public class GetTokenValidator : AbstractValidator<GetTokenQuery>
{
    public GetTokenValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
