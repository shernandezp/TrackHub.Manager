namespace TrackHub.Manager.Application.Operators.Queries.Get;

public class GetOperatorValidator : AbstractValidator<GetOperatorQuery>
{
    public GetOperatorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}

