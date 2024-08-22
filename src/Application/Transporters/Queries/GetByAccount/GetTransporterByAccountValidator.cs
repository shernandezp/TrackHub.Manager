namespace TrackHub.Manager.Application.Transporters.Queries.GetByAccount;

public class GetTransporterByAccountValidator : AbstractValidator<GetTransporterByAccountQuery>
{
    public GetTransporterByAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEmpty();
    }
}
