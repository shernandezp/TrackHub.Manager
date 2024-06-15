namespace TrackHub.Manager.Application.Accounts.Commands.Delete;

[Authorize(Resource = Resources.Accounts, Action = Actions.Delete)]
public record DeleteAccountCommand(Guid Id) : IRequest;

public class DeleteAccountCommandHandler(IAccountWriter writer) : IRequestHandler<DeleteAccountCommand>
{
    public async Task Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
        => await writer.DeleteAccountAsync(request.Id, cancellationToken);
}
