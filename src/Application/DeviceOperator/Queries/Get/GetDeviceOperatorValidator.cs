namespace TrackHub.Manager.Application.DeviceOperator.Queries.Get;

public class GetDeviceOperatorValidator : AbstractValidator<GetDeviceOperatorQuery>
{
    public GetDeviceOperatorValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}
