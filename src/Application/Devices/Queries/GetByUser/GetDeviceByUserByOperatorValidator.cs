namespace TrackHub.Manager.Application.Devices.Queries.GetByUser;

public class GetDeviceByUserByOperatorValidator : AbstractValidator<GetDeviceByUserByOperatorQuery>
{
    public GetDeviceByUserByOperatorValidator()
    {

        RuleFor(x => x.OperatorId)
            .NotEmpty();
    }
}
