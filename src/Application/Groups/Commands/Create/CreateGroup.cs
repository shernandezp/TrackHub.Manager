namespace TrackHub.Manager.Application.Groups.Commands.Create;

[Authorize(Resource = Resources.Groups, Action = Actions.Write)]
public readonly record struct CreateGroupCommand(GroupDto Group) : IRequest<GroupVm>;

public class CreateGroupCommandHandler(IGroupWriter writer) : IRequestHandler<CreateGroupCommand, GroupVm>
{
    public async Task<GroupVm> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        => await writer.CreateGroupAsync(request.Group, cancellationToken);
}
