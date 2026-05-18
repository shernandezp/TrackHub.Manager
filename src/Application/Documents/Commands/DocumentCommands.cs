namespace TrackHub.Manager.Application.Documents.Commands;

[Authorize(Resource = Resources.Documents, Action = Actions.Write)]
public readonly record struct CreateDocumentMetadataCommand(DocumentDto Document) : IRequest<DocumentVm>;
public class CreateDocumentMetadataCommandHandler(IDocumentWriter writer) : IRequestHandler<CreateDocumentMetadataCommand, DocumentVm>
{
    public async Task<DocumentVm> Handle(CreateDocumentMetadataCommand request, CancellationToken cancellationToken) => await writer.CreateDocumentMetadataAsync(request.Document, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Edit)]
public readonly record struct MarkDocumentUploadedCommand(Guid DocumentId, string Status) : IRequest;
public class MarkDocumentUploadedCommandHandler(IDocumentWriter writer) : IRequestHandler<MarkDocumentUploadedCommand>
{
    public async Task Handle(MarkDocumentUploadedCommand request, CancellationToken cancellationToken) => await writer.MarkDocumentUploadedAsync(request.DocumentId, request.Status, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Edit)]
public readonly record struct MarkDocumentScanResultCommand(Guid DocumentId, string ScanStatus) : IRequest;
public class MarkDocumentScanResultCommandHandler(IDocumentWriter writer) : IRequestHandler<MarkDocumentScanResultCommand>
{
    public async Task Handle(MarkDocumentScanResultCommand request, CancellationToken cancellationToken) => await writer.MarkDocumentScanResultAsync(request.DocumentId, request.ScanStatus, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Edit)]
public readonly record struct ExpireDocumentCommand(Guid DocumentId, DateTimeOffset ExpiresAt) : IRequest;
public class ExpireDocumentCommandHandler(IDocumentWriter writer) : IRequestHandler<ExpireDocumentCommand>
{
    public async Task Handle(ExpireDocumentCommand request, CancellationToken cancellationToken) => await writer.ExpireDocumentAsync(request.DocumentId, request.ExpiresAt, cancellationToken);
}

[Authorize(Resource = Resources.Documents, Action = Actions.Delete)]
public readonly record struct DeleteDocumentReferenceCommand(Guid DocumentId) : IRequest;
public class DeleteDocumentReferenceCommandHandler(IDocumentWriter writer) : IRequestHandler<DeleteDocumentReferenceCommand>
{
    public async Task Handle(DeleteDocumentReferenceCommand request, CancellationToken cancellationToken) => await writer.DeleteDocumentReferenceAsync(request.DocumentId, cancellationToken);
}
