using TrackHub.Manager.Application.Documents.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DocumentVm> CreateDocumentMetadata([Service] ISender sender, CreateDocumentMetadataCommand command) => await sender.Send(command);
    public async Task<bool> MarkDocumentUploaded([Service] ISender sender, MarkDocumentUploadedCommand command) { await sender.Send(command); return true; }
    public async Task<bool> MarkDocumentScanResult([Service] ISender sender, MarkDocumentScanResultCommand command) { await sender.Send(command); return true; }
    public async Task<bool> ExpireDocument([Service] ISender sender, ExpireDocumentCommand command) { await sender.Send(command); return true; }
    public async Task<Guid> DeleteDocumentReference([Service] ISender sender, DeleteDocumentReferenceCommand command) { await sender.Send(command); return command.DocumentId; }
}
