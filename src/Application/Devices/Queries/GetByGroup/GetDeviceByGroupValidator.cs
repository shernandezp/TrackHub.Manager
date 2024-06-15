namespace TrackHub.Manager.Application.Devices.Queries.GetByGroup;

public class GetDeviceByGroupValidator : AbstractValidator<GetDeviceByGroupQuery>
{
    public GetDeviceByGroupValidator()
    {
        RuleFor(x => x.GroupId)
            .NotEmpty();
    }
}
