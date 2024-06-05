namespace TrackHub.Manager.Application.Users.Queries.GetUser;

public class GetUserValidator : AbstractValidator<GetUserQuery>
{
    public GetUserValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
