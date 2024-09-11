namespace TrackHub.Manager.Application.Users.Queries.GetByGroup;

public class GetUsersByGroupValidator : AbstractValidator<GetUsersByGroupQuery>
{
    public GetUsersByGroupValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
