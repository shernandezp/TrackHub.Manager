namespace TrackHub.Manager.Application.Users.Queries.GetByAccount;

public class GetUserByAccountValidator : AbstractValidator<GetUsersByAccountQuery>
{
    public GetUserByAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
    }
}
