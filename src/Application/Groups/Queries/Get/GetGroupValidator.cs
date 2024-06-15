namespace TrackHub.Manager.Application.Groups.Queries.Get;

public class GetGroupValidator : AbstractValidator<GetGroupQuery>
{
    public GetGroupValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
