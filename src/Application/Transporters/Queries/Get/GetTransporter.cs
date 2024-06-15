namespace TrackHub.Manager.Application.Transporters.Queries.Get;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetTransporterQuery(Guid Id) : IRequest<TransporterVm>;

public class GetTransportersQueryHandler(ITransporterReader reader) : IRequestHandler<GetTransporterQuery, TransporterVm>
{
    public async Task<TransporterVm> Handle(GetTransporterQuery request, CancellationToken cancellationToken)
        => await reader.GetTransporterAsync(request.Id, cancellationToken);

}
