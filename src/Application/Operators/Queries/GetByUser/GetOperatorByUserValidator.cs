namespace TrackHub.Manager.Application.Operators.Queries.GetByUser;

public class GetOperatorByUserValidator : AbstractValidator<GetOperatorByUserQuery>
{
    public GetOperatorByUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
