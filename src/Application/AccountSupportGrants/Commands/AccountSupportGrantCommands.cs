namespace TrackHub.Manager.Application.AccountSupportGrants.Commands;

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Write)]
public readonly record struct CreateAccountSupportGrantCommand(AccountSupportGrantDto AccountSupportGrant) : IRequest<AccountSupportGrantVm>;
public class CreateAccountSupportGrantCommandHandler(IAccountSupportGrantWriter writer) : IRequestHandler<CreateAccountSupportGrantCommand, AccountSupportGrantVm>
{
    public async Task<AccountSupportGrantVm> Handle(CreateAccountSupportGrantCommand request, CancellationToken cancellationToken) => await writer.CreateAccountSupportGrantAsync(request.AccountSupportGrant, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Edit)]
public readonly record struct ApproveAccountSupportGrantCommand(Guid AccountSupportGrantId, string ApprovedBy) : IRequest;
public class ApproveAccountSupportGrantCommandHandler(IAccountSupportGrantWriter writer) : IRequestHandler<ApproveAccountSupportGrantCommand>
{
    public async Task Handle(ApproveAccountSupportGrantCommand request, CancellationToken cancellationToken) => await writer.ApproveAccountSupportGrantAsync(request.AccountSupportGrantId, request.ApprovedBy, cancellationToken);
}

[Authorize(Resource = Resources.SupportGrants, Action = Actions.Delete)]
public readonly record struct RevokeAccountSupportGrantCommand(Guid AccountSupportGrantId, string RevokedBy) : IRequest;
public class RevokeAccountSupportGrantCommandHandler(IAccountSupportGrantWriter writer) : IRequestHandler<RevokeAccountSupportGrantCommand>
{
    public async Task Handle(RevokeAccountSupportGrantCommand request, CancellationToken cancellationToken) => await writer.RevokeAccountSupportGrantAsync(request.AccountSupportGrantId, request.RevokedBy, cancellationToken);
}
