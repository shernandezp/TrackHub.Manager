namespace TrackHub.Manager.Application.Accounts.Commands.Disable;

// This command requires authorization to delete an account
[Authorize(Resource = Resources.Accounts, Action = Actions.Delete)]
public record DisableAccountCommand(Guid Id) : IRequest;

public class DisableAccountCommandHandler(IAccountWriter writer) : IRequestHandler<DisableAccountCommand>
{
    // Handles the delete account command
    public async Task Handle(DisableAccountCommand request, CancellationToken cancellationToken)
        => await writer.DisableAccountAsync(request.Id, cancellationToken);
}
