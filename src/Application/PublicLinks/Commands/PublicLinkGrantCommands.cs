namespace TrackHub.Manager.Application.PublicLinks.Commands;

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Write)]
public readonly record struct CreatePublicLinkGrantCommand(PublicLinkGrantDto PublicLinkGrant) : IRequest<PublicLinkGrantVm>;
public class CreatePublicLinkGrantCommandHandler(IPublicLinkGrantWriter writer) : IRequestHandler<CreatePublicLinkGrantCommand, PublicLinkGrantVm>
{
    public async Task<PublicLinkGrantVm> Handle(CreatePublicLinkGrantCommand request, CancellationToken cancellationToken) => await writer.CreatePublicLinkGrantAsync(request.PublicLinkGrant, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Delete)]
public readonly record struct RevokePublicLinkGrantCommand(Guid PublicLinkGrantId, string RevokedBy) : IRequest;
public class RevokePublicLinkGrantCommandHandler(IPublicLinkGrantWriter writer) : IRequestHandler<RevokePublicLinkGrantCommand>
{
    public async Task Handle(RevokePublicLinkGrantCommand request, CancellationToken cancellationToken) => await writer.RevokePublicLinkGrantAsync(request.PublicLinkGrantId, request.RevokedBy, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Read)]
public readonly record struct RecordPublicLinkAccessCommand(Guid PublicLinkGrantId) : IRequest;
public class RecordPublicLinkAccessCommandHandler(IPublicLinkGrantWriter writer) : IRequestHandler<RecordPublicLinkAccessCommand>
{
    public async Task Handle(RecordPublicLinkAccessCommand request, CancellationToken cancellationToken) => await writer.RecordPublicLinkAccessAsync(request.PublicLinkGrantId, cancellationToken);
}
