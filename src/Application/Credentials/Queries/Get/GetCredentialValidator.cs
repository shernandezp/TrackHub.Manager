namespace TrackHub.Manager.Application.Credentials.Queries.Get;

public class GetCredentialValidator : AbstractValidator<GetCredentialQuery>
{
    public GetCredentialValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
