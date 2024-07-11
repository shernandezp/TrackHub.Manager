namespace TrackHub.Manager.Infrastructure.Readers;
public sealed class OperatorReader(IApplicationDbContext context) : IOperatorReader
{
    public async Task<OperatorVm> GetOperatorAsync(Guid id, CancellationToken cancellationToken)
        => await context.Operators
            .Include(o => o.Credential)
            .Where(o => o.OperatorId.Equals(id))
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                o.ProtocolType,
                o.Credential == null ? null : new CredentialTokenVm(
                    o.Credential.CredentialId,
                    o.Credential.Uri,
                    o.Credential.Username,
                    o.Credential.Password,
                    o.Credential.Salt,
                    o.Credential.Key,
                    o.Credential.Key2,
                    o.Credential.Token,
                    o.Credential.TokenExpiration,
                    o.Credential.RefreshToken,
                    o.Credential.RefreshTokenExpiration)))
            .FirstAsync(cancellationToken);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByAccountAsync(Guid accountId, CancellationToken cancellationToken)
        => await context.Operators
            .Where(o => o.AccountId == accountId)
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                o.ProtocolType,
                null))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByUserAsync(Guid userId, CancellationToken cancellationToken)
        => await context.Users
            .Where(u => u.UserId == userId)
            .SelectMany(u => u.Groups)
            .SelectMany(g => g.Devices)
            .SelectMany(d => d.Operators)
            .Distinct()
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                o.ProtocolType,
                o.Credential == null ? null : new CredentialTokenVm(
                    o.Credential.CredentialId,
                    o.Credential.Uri,
                    o.Credential.Username,
                    o.Credential.Password,
                    o.Credential.Salt,
                    o.Credential.Key,
                    o.Credential.Key2,
                    o.Credential.Token,
                    o.Credential.TokenExpiration,
                    o.Credential.RefreshToken,
                    o.Credential.RefreshTokenExpiration)))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyCollection<OperatorVm>> GetOperatorsByGroupAsync(long groupId, CancellationToken cancellationToken)
        => await context.Groups
            .Where(u => u.GroupId == groupId)
            .SelectMany(g => g.Devices)
            .SelectMany(d => d.Operators)
            .Distinct()
            .Select(o => new OperatorVm(
                o.OperatorId,
                o.Name,
                o.Description,
                o.PhoneNumber,
                o.EmailAddress,
                o.Address,
                o.ContactName,
                o.ProtocolType,
                o.Credential == null ? null : new CredentialTokenVm(
                    o.Credential.CredentialId,
                    o.Credential.Uri,
                    o.Credential.Username,
                    o.Credential.Password,
                    o.Credential.Salt,
                    o.Credential.Key,
                    o.Credential.Key2,
                    o.Credential.Token,
                    o.Credential.TokenExpiration,
                    o.Credential.RefreshToken,
                    o.Credential.RefreshTokenExpiration)))
            .ToListAsync(cancellationToken);
}
