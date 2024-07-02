namespace TrackHub.Manager.Application.Operators.Queries.GetByGroup;

public class GetOperatorByGroupValidator : AbstractValidator<GetOperatorByGroupQuery>
{
    public GetOperatorByGroupValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
