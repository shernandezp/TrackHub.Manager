namespace TrackHub.Manager.Application.Credentials.Queries.GetByOperator;

public class GetCredentialByOperatorValidator : AbstractValidator<GetCredentialByOperatorQuery>
{
    public GetCredentialByOperatorValidator()
    {
        RuleFor(x => x.OperatorId)
            .NotEmpty();
    }
}
