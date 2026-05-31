namespace TrackHub.Manager.Application.GpsIntegration.Commands;

public class SetPositionRetentionPolicyCommandValidator : AbstractValidator<SetPositionRetentionPolicyCommand>
{
    public SetPositionRetentionPolicyCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.Policy.RetentionDays).InclusiveBetween(1, 3650);
    }
}
