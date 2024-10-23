using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.TransporterPosition.Queries.GetByOperator;

[Authorize(Resource = Resources.Devices, Action = Actions.Read)]
public readonly record struct GetTransporterPositionsByOperatorQuery(Guid OperatorId) : IRequest<IReadOnlyCollection<TransporterPositionVm>>;

public class GetTransporterPositionsByOperatorQueryHandler(ITransporterPositionReader reader, IUser user) : IRequestHandler<GetTransporterPositionsByOperatorQuery, IReadOnlyCollection<TransporterPositionVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<TransporterPositionVm>> Handle(GetTransporterPositionsByOperatorQuery request, CancellationToken cancellationToken)
        => await reader.GetTransporterPositionsAsync(UserId, request.OperatorId, cancellationToken);

}
