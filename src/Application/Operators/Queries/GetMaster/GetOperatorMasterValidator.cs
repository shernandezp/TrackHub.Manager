namespace TrackHub.Manager.Application.Operators.Queries.GetMaster;

public class GetOperatorMasterValidator : AbstractValidator<GetOperatorMasterQuery>
{
    public GetOperatorMasterValidator()
    {
        RuleFor(x => x.Filter)
            .NotEmpty();
    }
}
