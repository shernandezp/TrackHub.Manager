using TrackHub.Manager.Application.Documents.Queries;

namespace TrackHub.Manager.Web.GraphQL.Query;

public partial class Query
{
    public async Task<IReadOnlyCollection<DocumentVm>> GetDocumentsForOwner([Service] ISender sender, [AsParameters] GetDocumentsForOwnerQuery query) => await sender.Send(query);
}
