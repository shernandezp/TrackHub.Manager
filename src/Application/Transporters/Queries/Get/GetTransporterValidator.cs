namespace TrackHub.Manager.Application.Transporters.Queries.Get;

public class GetTransporterValidator : AbstractValidator<GetTransporterQuery>
{
    public GetTransporterValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
