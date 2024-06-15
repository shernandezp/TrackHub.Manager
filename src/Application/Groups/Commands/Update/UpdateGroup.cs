namespace TrackHub.Manager.Application.Groups.Commands.Update;

[Authorize(Resource = Resources.Groups, Action = Actions.Edit)]
public readonly record struct UpdateGroupCommand(UpdateGroupDto Group) : IRequest;

public class UpdateGroupCommandHandler(IGroupWriter writer) : IRequestHandler<UpdateGroupCommand>
{
    public async Task Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        => await writer.UpdateGroupAsync(request.Group, cancellationToken);
}
