namespace TrackHub.Manager.Application.Transporters.Queries.GetByAccount;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetTransporterByAccountQuery(Guid AccountId) : IRequest<IReadOnlyCollection<TransporterVm>>;

public class GetTransportersQueryHandler(ITransporterReader reader) : IRequestHandler<GetTransporterByAccountQuery, IReadOnlyCollection<TransporterVm>>
{
    public async Task<IReadOnlyCollection<TransporterVm>> Handle(GetTransporterByAccountQuery request, CancellationToken cancellationToken)
        => await reader.GetTransportersByAccountAsync(request.AccountId, cancellationToken);

}
