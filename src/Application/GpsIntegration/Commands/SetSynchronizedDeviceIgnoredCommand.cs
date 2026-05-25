namespace TrackHub.Manager.Application.GpsIntegration.Commands;

[Authorize(Resource = Resources.SynchronizedDevices, Action = Actions.Edit)]

public readonly record struct SetSynchronizedDeviceIgnoredCommand(Guid DeviceId, bool Ignored) : IRequest;

public class SetSynchronizedDeviceIgnoredCommandHandler(IDeviceWriter writer)
    : IRequestHandler<SetSynchronizedDeviceIgnoredCommand>
{
    public Task Handle(SetSynchronizedDeviceIgnoredCommand request, CancellationToken cancellationToken)
        => writer.SetDetectedStatusAsync(request.DeviceId,
            request.Ignored ? DetectedStatus.Ignored : DetectedStatus.Available, cancellationToken);
}
