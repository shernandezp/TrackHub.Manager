namespace TrackHub.Manager.Application.Accounts.Queries.Get;
public class GetAccountValidator : AbstractValidator<GetAccountQuery>
{
    public GetAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
