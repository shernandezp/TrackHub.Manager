namespace TrackHub.Manager.Application.Operators.Commands.Delete;

[Authorize(Resource = Resources.Operators, Action = Actions.Delete)]
public record DeleteOperatorCommand(Guid Id) : IRequest;

public class DeleteOperatorCommandHandler(IOperatorWriter writer, IOperatorReader reader, ICredentialWriter credentialWriter) : IRequestHandler<DeleteOperatorCommand>
{
    public async Task Handle(DeleteOperatorCommand request, CancellationToken cancellationToken) 
    {
        var @operator = await reader.GetOperatorAsync(request.Id, cancellationToken);
        if (@operator.Credential != null && @operator.Credential != default(CredentialTokenVm)) 
        {
            await credentialWriter.DeleteCredentialAsync(@operator.Credential.Value.CredentialId, cancellationToken);
        }

        await writer.DeleteOperatorAsync(request.Id, cancellationToken); 
    }
}
