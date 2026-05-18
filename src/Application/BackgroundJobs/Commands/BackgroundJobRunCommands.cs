namespace TrackHub.Manager.Application.BackgroundJobs.Commands;

[Authorize(Resource = Resources.BackgroundJobs, Action = Actions.Write)]
public readonly record struct CreateBackgroundJobRunCommand(BackgroundJobRunDto BackgroundJobRun) : IRequest<BackgroundJobRunVm>;
public class CreateBackgroundJobRunCommandHandler(IBackgroundJobRunWriter writer) : IRequestHandler<CreateBackgroundJobRunCommand, BackgroundJobRunVm>
{
    public async Task<BackgroundJobRunVm> Handle(CreateBackgroundJobRunCommand request, CancellationToken cancellationToken) => await writer.CreateBackgroundJobRunAsync(request.BackgroundJobRun, cancellationToken);
}
