namespace TrackHub.Manager.Application.GpsIntegration.Queries;

[Authorize(Resource = Resources.Credentials, Action = Actions.Read)]

public readonly record struct GetOperatorCredentialMetadataQuery(Guid OperatorId) : IRequest<CredentialMetadataVm?>;

public class GetOperatorCredentialMetadataQueryHandler(ICredentialReader reader)
    : IRequestHandler<GetOperatorCredentialMetadataQuery, CredentialMetadataVm?>
{
    public Task<CredentialMetadataVm?> Handle(GetOperatorCredentialMetadataQuery request, CancellationToken cancellationToken)
        => reader.GetMetadataByOperatorAsync(request.OperatorId, cancellationToken);
}
