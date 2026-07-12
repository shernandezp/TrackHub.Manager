using TrackHub.Manager.Application.Documents.Commands;

namespace TrackHub.Manager.Web.GraphQL.Mutation;

public partial class Mutation
{
    public async Task<DocumentVm> CreateDocumentMetadata([Service] ISender sender, CreateDocumentMetadataCommand command) => await sender.Send(command);
    public async Task<bool> MarkDocumentUploaded([Service] ISender sender, MarkDocumentUploadedCommand command) { await sender.Send(command); return true; }
    public async Task<bool> MarkDocumentScanResult([Service] ISender sender, MarkDocumentScanResultCommand command) { await sender.Send(command); return true; }
    public async Task<bool> ExpireDocument([Service] ISender sender, ExpireDocumentCommand command) { await sender.Send(command); return true; }
    public async Task<Guid> DeleteDocumentReference([Service] ISender sender, DeleteDocumentReferenceCommand command) { await sender.Send(command); return command.DocumentId; }
    public async Task<DocumentVm> ReplaceDocumentVersion([Service] ISender sender, ReplaceDocumentVersionCommand command) => await sender.Send(command);
    public async Task<bool> VoidDocument([Service] ISender sender, VoidDocumentCommand command) { await sender.Send(command); return true; }
    public async Task<DocumentSignatureVm> SignDocument([Service] ISender sender, SignDocumentCommand command) => await sender.Send(command);
    public async Task<DocumentTypeVm> ConfigureDocumentType([Service] ISender sender, ConfigureDocumentTypeCommand command) => await sender.Send(command);
    public async Task<Guid> DisableDocumentType([Service] ISender sender, DisableDocumentTypeCommand command) { await sender.Send(command); return command.DocumentTypeId; }
}
