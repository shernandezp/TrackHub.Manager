namespace TrackHub.Manager.Application.Groups.Queries.GetByAccount;

public class GetGroupByAccountValidator : AbstractValidator<GetGroupByAccountQuery>
{
    public GetGroupByAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
    }
}
