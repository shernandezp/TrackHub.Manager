using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Transporters.Queries.GetByUser;

[Authorize(Resource = Resources.Transporters, Action = Actions.Read)]
public readonly record struct GetTransporterByUserQuery() : IRequest<IReadOnlyCollection<TransporterVm>>;

public class GetTransportersQueryHandler(ITransporterReader reader, IUser user) : IRequestHandler<GetTransporterByUserQuery, IReadOnlyCollection<TransporterVm>>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<IReadOnlyCollection<TransporterVm>> Handle(GetTransporterByUserQuery request, CancellationToken cancellationToken)
        => await reader.GetTransportersByUserAsync(UserId, cancellationToken);

}
