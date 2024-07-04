namespace TrackHub.Manager.Application.Devices.Queries.GetByGroup;

public class GetDeviceByGroupByOperatorValidator : AbstractValidator<GetDeviceByGroupByOperatorQuery>
{
    public GetDeviceByGroupByOperatorValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();

        RuleFor(x => x.OperatorId)
            .NotEmpty();
    }
}
