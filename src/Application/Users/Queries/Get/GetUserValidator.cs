namespace TrackHub.Manager.Application.Users.Queries.Get;

public class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
