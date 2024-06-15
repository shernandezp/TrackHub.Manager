namespace TrackHub.Manager.Application.Operators.Queries.GetByAccount;

public class GetOperatorByAccountValidator : AbstractValidator<GetOperatorByAccountQuery>
{
    public GetOperatorByAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
    }
}
