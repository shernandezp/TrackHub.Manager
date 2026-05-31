using TrackHub.Manager.Infrastructure.Interfaces;
using TrackHub.Manager.Infrastructure.ManagerDB.Readers;
using TrackHub.Manager.Infrastructure;
using Common.Domain.Helpers;
using Common.Application.Interfaces;
using Common.Domain.Constants;
using Moq;

namespace Infrastructure.UnitTests;

[TestFixture]
public class OperatorReaderTests
{
    private static ApplicationDbContext NewContext(string name)
        => new(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(name).Options);

    private static ICurrentPrincipal Principal(PrincipalType principalType = PrincipalType.User, Guid? userId = null)
    {
        var principal = new Mock<ICurrentPrincipal>();
        principal.SetupGet(p => p.PrincipalType).Returns(principalType);
        principal.SetupGet(p => p.UserId).Returns(userId);
        return principal.Object;
    }

    private static Mock<IIdentityService> IdentityService(bool roleAuthorized = false, bool policyAuthorized = false)
    {
        var identityService = new Mock<IIdentityService>();
        identityService
            .Setup(x => x.IsInRoleAsync(It.IsAny<Guid>(), Resources.Credentials, Actions.Custom, It.IsAny<CancellationToken>()))
            .ReturnsAsync(roleAuthorized);
        identityService
            .Setup(x => x.AuthorizeAsync(It.IsAny<Guid>(), Resources.Credentials, Actions.Custom, It.IsAny<CancellationToken>()))
            .ReturnsAsync(policyAuthorized);
        return identityService;
    }

    [Test]
    public async Task GetOperatorsByUserAsync_ReturnsOperatorsForUserAccount()
    {
        var accountId = Guid.NewGuid();
        var otherAccount = Guid.NewGuid();
        var userId = Guid.NewGuid();
        await using var context = NewContext(nameof(GetOperatorsByUserAsync_ReturnsOperatorsForUserAccount));

        var user = new User(userId, "alice", true, accountId);
        var opIncluded = new Operator("op1", null, null, null, null, null, 1, accountId);
        var opExcluded = new Operator("op2", null, null, null, null, null, 1, otherAccount);

        await context.Users.AddAsync(user);
        await context.Operators.AddRangeAsync(opIncluded, opExcluded);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext, Principal(), IdentityService().Object);
        var result = await reader.GetOperatorsByUserAsync(userId, CancellationToken.None);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.First().OperatorId, Is.EqualTo(opIncluded.OperatorId));
    }

    [Test]
    public async Task GetOperatorAsync_RedactsCredentialForUserPrincipal()
    {
        await using var context = NewContext(nameof(GetOperatorAsync_RedactsCredentialForUserPrincipal));

        var accountId = Guid.NewGuid();
        var @operator = new Operator("opC", null, null, null, null, null, 1, accountId);
        var credential = new Credential("http://uri", "user", "pass", null, null, "salt", @operator.OperatorId);
        @operator.Credential = credential;

        await context.Operators.AddAsync(@operator);
        await context.Credentials.AddAsync(credential);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext, Principal(userId: Guid.NewGuid()), IdentityService().Object);
        var result = await reader.GetOperatorAsync(@operator.OperatorId, CancellationToken.None);

        Assert.That(result.OperatorId, Is.EqualTo(@operator.OperatorId));
        Assert.That(result.Credential.HasValue, Is.False);
    }

    [Test]
    public async Task GetOperatorAsync_IncludesCredentialForUserWithCredentialsCustomPermission()
    {
        await using var context = NewContext(nameof(GetOperatorAsync_IncludesCredentialForUserWithCredentialsCustomPermission));

        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var @operator = new Operator("opC", null, null, null, null, null, 1, accountId);
        var credential = new Credential("http://uri", "user", "pass", null, null, "salt", @operator.OperatorId);
        @operator.Credential = credential;

        await context.Operators.AddAsync(@operator);
        await context.Credentials.AddAsync(credential);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(
            context as IApplicationDbContext,
            Principal(userId: userId),
            IdentityService(roleAuthorized: true, policyAuthorized: true).Object);
        var result = await reader.GetOperatorAsync(@operator.OperatorId, CancellationToken.None);

        Assert.That(result.OperatorId, Is.EqualTo(@operator.OperatorId));
        Assert.That(result.Credential.HasValue, Is.True);
        Assert.That(result.Credential!.Value.Username, Is.EqualTo(credential.Username));
    }

    [Test]
    public async Task GetOperatorAsync_IncludesCredentialForServiceClientPrincipal()
    {
        await using var context = NewContext(nameof(GetOperatorAsync_IncludesCredentialForServiceClientPrincipal));

        var accountId = Guid.NewGuid();
        var @operator = new Operator("opC", null, null, null, null, null, 1, accountId);
        var credential = new Credential("http://uri", "user", "pass", null, null, "salt", @operator.OperatorId);
        @operator.Credential = credential;

        await context.Operators.AddAsync(@operator);
        await context.Credentials.AddAsync(credential);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext, Principal(PrincipalType.ServiceClient), IdentityService().Object);
        var result = await reader.GetOperatorAsync(@operator.OperatorId, CancellationToken.None);

        Assert.That(result.OperatorId, Is.EqualTo(@operator.OperatorId));
        Assert.That(result.Credential.HasValue, Is.True);
        Assert.That(result.Credential!.Value.Username, Is.EqualTo(credential.Username));
    }

    [Test]
    public async Task GetOperatorAsync_NotFound_Throws()
    {
        await using var context = NewContext(nameof(GetOperatorAsync_NotFound_Throws));
        var reader = new OperatorReader(context as IApplicationDbContext, Principal(), IdentityService().Object);

        Assert.ThrowsAsync<NotFoundException>(async () =>
            await reader.GetOperatorAsync(Guid.NewGuid(), CancellationToken.None));
    }

    [Test]
    public async Task GetOperatorsAsync_AppliesFilters()
    {
        await using var context = NewContext(nameof(GetOperatorsAsync_AppliesFilters));

        var accountId = Guid.NewGuid();
        var otherAccount = Guid.NewGuid();
        var op1 = new Operator("op1", null, null, null, null, null, 1, accountId);
        var op2 = new Operator("op2", null, null, null, null, null, 1, otherAccount);

        await context.Operators.AddRangeAsync(op1, op2);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext, Principal(), IdentityService().Object);
        var filters = new Filters(new Dictionary<string, object> { { "AccountId", accountId } });
        var result = await reader.GetOperatorsAsync(filters, CancellationToken.None);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.First().AccountId, Is.EqualTo(accountId));
    }
}
