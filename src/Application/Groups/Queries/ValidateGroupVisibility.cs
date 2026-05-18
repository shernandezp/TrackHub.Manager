namespace TrackHub.Manager.Application.Groups.Queries;

[Authorize(Resource = Resources.Groups, Action = Actions.Read)]
public readonly record struct ValidateGroupVisibilityQuery(Guid AccountId, Guid UserId, string ResourceType, string ResourceId) : IRequest<bool>;
public class ValidateGroupVisibilityQueryHandler(IGroupVisibilityReader reader) : IRequestHandler<ValidateGroupVisibilityQuery, bool>
{
    public async Task<bool> Handle(ValidateGroupVisibilityQuery request, CancellationToken cancellationToken) => await reader.ValidateGroupVisibilityAsync(request.AccountId, request.UserId, request.ResourceType, request.ResourceId, cancellationToken);
}
