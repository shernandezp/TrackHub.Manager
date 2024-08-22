namespace TrackHub.Manager.Application.Transporters.Queries.GetByGroup;

public class GetTransporterByGroupValidator : AbstractValidator<GetTransporterByGroupQuery>
{
    public GetTransporterByGroupValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
