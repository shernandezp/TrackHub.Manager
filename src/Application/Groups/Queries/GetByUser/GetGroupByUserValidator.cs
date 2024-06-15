namespace TrackHub.Manager.Application.Groups.Queries.GetByUser;

public class GetGroupByUserValidator : AbstractValidator<GetGroupByUserQuery>
{
    public GetGroupByUserValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty();
    }
}
