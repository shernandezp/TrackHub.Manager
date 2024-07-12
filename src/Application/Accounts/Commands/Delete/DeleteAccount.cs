namespace TrackHub.Manager.Application.Accounts.Commands.Delete;

// This command requires authorization to delete an account
[Authorize(Resource = Resources.Accounts, Action = Actions.Delete)]
public record DeleteAccountCommand(Guid Id) : IRequest;

public class DeleteAccountCommandHandler(IAccountWriter writer) : IRequestHandler<DeleteAccountCommand>
{
    // Handles the delete account command
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        => await writer.DeleteAccountAsync(request.Id, cancellationToken);
}
