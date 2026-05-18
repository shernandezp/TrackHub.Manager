namespace TrackHub.Manager.Application.PublicLinks.Queries;

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Read)]
public readonly record struct GetPublicLinkGrantQuery(Guid PublicLinkGrantId) : IRequest<PublicLinkGrantVm>;
public class GetPublicLinkGrantQueryHandler(IPublicLinkGrantReader reader) : IRequestHandler<GetPublicLinkGrantQuery, PublicLinkGrantVm>
{
    public async Task<PublicLinkGrantVm> Handle(GetPublicLinkGrantQuery request, CancellationToken cancellationToken) => await reader.GetPublicLinkGrantAsync(request.PublicLinkGrantId, cancellationToken);
}

[Authorize(Resource = Resources.PublicLinks, Action = Actions.Read)]
public readonly record struct GetPublicLinkGrantsByAccountQuery(Guid AccountId, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<PublicLinkGrantVm>>;
public class GetPublicLinkGrantsByAccountQueryHandler(IPublicLinkGrantReader reader) : IRequestHandler<GetPublicLinkGrantsByAccountQuery, IReadOnlyCollection<PublicLinkGrantVm>>
{
    public async Task<IReadOnlyCollection<PublicLinkGrantVm>> Handle(GetPublicLinkGrantsByAccountQuery request, CancellationToken cancellationToken) => await reader.GetPublicLinkGrantsByAccountAsync(request.AccountId, request.Skip, request.Take, cancellationToken);
}
