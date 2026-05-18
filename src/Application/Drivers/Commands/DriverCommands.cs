namespace TrackHub.Manager.Application.Drivers.Commands;

[Authorize(Resource = Resources.Drivers, Action = Actions.Write)]
public readonly record struct CreateDriverCommand(DriverDto Driver) : IRequest<DriverVm>;
public class CreateDriverCommandHandler(IDriverWriter writer) : IRequestHandler<CreateDriverCommand, DriverVm>
{
    public async Task<DriverVm> Handle(CreateDriverCommand request, CancellationToken cancellationToken) => await writer.CreateDriverAsync(request.Driver, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Edit)]
public readonly record struct UpdateDriverCommand(Guid DriverId, DriverDto Driver) : IRequest;
public class UpdateDriverCommandHandler(IDriverWriter writer) : IRequestHandler<UpdateDriverCommand>
{
    public async Task Handle(UpdateDriverCommand request, CancellationToken cancellationToken) => await writer.UpdateDriverAsync(request.DriverId, request.Driver, cancellationToken);
}

[Authorize(Resource = Resources.Drivers, Action = Actions.Delete)]
public readonly record struct DeactivateDriverCommand(Guid DriverId) : IRequest;
public class DeactivateDriverCommandHandler(IDriverWriter writer) : IRequestHandler<DeactivateDriverCommand>
{
    public async Task Handle(DeactivateDriverCommand request, CancellationToken cancellationToken) => await writer.DeactivateDriverAsync(request.DriverId, cancellationToken);
}
