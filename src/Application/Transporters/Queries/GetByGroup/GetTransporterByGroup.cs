namespace TrackHub.Manager.Application.Transporters.Queries.GetByGroup;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetTransporterByGroupQuery(long GroupId) : IRequest<IReadOnlyCollection<TransporterVm>>;

public class GetTransportersQueryHandler(ITransporterReader reader) : IRequestHandler<GetTransporterByGroupQuery, IReadOnlyCollection<TransporterVm>>
{
    public async Task<IReadOnlyCollection<TransporterVm>> Handle(GetTransporterByGroupQuery request, CancellationToken cancellationToken)
        => await reader.GetTransportersByGroupAsync(request.GroupId, cancellationToken);

}
