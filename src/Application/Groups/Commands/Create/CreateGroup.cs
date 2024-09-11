using Common.Application.Interfaces;

namespace TrackHub.Manager.Application.Groups.Commands.Create;

[Authorize(Resource = Resources.Groups, Action = Actions.Write)]
public readonly record struct CreateGroupCommand(GroupDto Group) : IRequest<GroupVm>;

public class CreateGroupCommandHandler(IGroupWriter writer, IUserReader userReader, IUser user) : IRequestHandler<CreateGroupCommand, GroupVm>
{
    private Guid UserId { get; } = user.Id is null ? throw new UnauthorizedAccessException() : new Guid(user.Id);

    public async Task<GroupVm> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
    {
        var user = await userReader.GetUserAsync(UserId, cancellationToken);
        return await writer.CreateGroupAsync(request.Group, user.AccountId, cancellationToken);
    }
}
