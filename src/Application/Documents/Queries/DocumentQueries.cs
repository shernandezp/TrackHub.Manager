namespace TrackHub.Manager.Application.Documents.Queries;

[Authorize(Resource = Resources.Documents, Action = Actions.Read)]
public readonly record struct GetDocumentsForOwnerQuery(Guid AccountId, string OwnerEntityType, string OwnerEntityId, DateTimeOffset? From = null, DateTimeOffset? To = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<DocumentVm>>;
public class GetDocumentsForOwnerQueryHandler(IDocumentReader reader) : IRequestHandler<GetDocumentsForOwnerQuery, IReadOnlyCollection<DocumentVm>>
{
    public async Task<IReadOnlyCollection<DocumentVm>> Handle(GetDocumentsForOwnerQuery request, CancellationToken cancellationToken) => await reader.GetDocumentsForOwnerAsync(request.AccountId, request.OwnerEntityType, request.OwnerEntityId, request.From, request.To, request.Skip, request.Take, cancellationToken);
}
