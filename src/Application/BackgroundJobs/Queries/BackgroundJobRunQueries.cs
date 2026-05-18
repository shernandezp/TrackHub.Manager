namespace TrackHub.Manager.Application.BackgroundJobs.Queries;

[Authorize(Resource = Resources.BackgroundJobs, Action = Actions.Read)]
public readonly record struct GetBackgroundJobRunsQuery(Guid? AccountId, DateTimeOffset? From = null, DateTimeOffset? To = null, int Skip = 0, int Take = 50) : IRequest<IReadOnlyCollection<BackgroundJobRunVm>>;
public class GetBackgroundJobRunsQueryHandler(IBackgroundJobRunReader reader) : IRequestHandler<GetBackgroundJobRunsQuery, IReadOnlyCollection<BackgroundJobRunVm>>
{
    public async Task<IReadOnlyCollection<BackgroundJobRunVm>> Handle(GetBackgroundJobRunsQuery request, CancellationToken cancellationToken) => await reader.GetBackgroundJobRunsAsync(request.AccountId, request.From, request.To, request.Skip, request.Take, cancellationToken);
}
