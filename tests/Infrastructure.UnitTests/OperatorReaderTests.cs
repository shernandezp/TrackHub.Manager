using TrackHub.Manager.Infrastructure.Interfaces;
using TrackHub.Manager.Infrastructure.ManagerDB.Readers;
using TrackHub.Manager.Infrastructure;
using Common.Domain.Helpers;

namespace Infrastructure.UnitTests;

[TestFixture]
public class OperatorReaderTests
{
    [Test]
    public async Task GetOperatorsByUserAsync_ReturnsOnlyOperatorsFromUserAccountAndGroups()
    {
        // Arrange
        var dbName = nameof(GetOperatorsByUserAsync_ReturnsOnlyOperatorsFromUserAccountAndGroups);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var accountId = Guid.NewGuid();
        var otherAccountId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        var user = new User(userId, "alice", true, accountId);
        var group = new Group("group1", "desc", true, accountId);
        var transporter = new Transporter("transp", 1);

        // Link relations
        group.Users.Add(user);
        transporter.Groups.Add(group);
        group.Transporters.Add(transporter);

        var operatorIncluded = new Operator("op1", null, null, null, null, null, 1, accountId);
        var device1 = new Device("dev1", 1, "s1", 1, null, transporter.TransporterId, operatorIncluded.OperatorId)
        {
            Transporter = transporter,
            Operator = operatorIncluded
        };
        transporter.Devices.Add(device1);
        operatorIncluded.Devices.Add(device1);

        var operatorExcluded = new Operator("op2", null, null, null, null, null, 1, otherAccountId);
        var device2 = new Device("dev2", 2, "s2", 1, null, transporter.TransporterId, operatorExcluded.OperatorId)
        {
            Transporter = transporter,
            Operator = operatorExcluded
        };
        transporter.Devices.Add(device2);
        operatorExcluded.Devices.Add(device2);

        // Add to context
        await context.Users.AddAsync(user);
        await context.Groups.AddAsync(group);
        await context.Transporters.AddAsync(transporter);
        await context.Operators.AddRangeAsync(operatorIncluded, operatorExcluded);
        await context.Devices.AddRangeAsync(device1, device2);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext);

        // Act
        var result = await reader.GetOperatorsByUserAsync(userId, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.First().OperatorId, Is.EqualTo(operatorIncluded.OperatorId));
    }

    [Test]
    public async Task GetOperatorsByUserAsync_ReturnsDistinctOperators_WhenMultipleDevices()
    {
        // Arrange
        var dbName = nameof(GetOperatorsByUserAsync_ReturnsDistinctOperators_WhenMultipleDevices);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var accountId = Guid.NewGuid();
        var userId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        var user = new User(userId, "bob", true, accountId);
        var group = new Group("groupA", "desc", true, accountId);
        var transporter1 = new Transporter("t1", 1);
        var transporter2 = new Transporter("t2", 1);

        group.Users.Add(user);
        transporter1.Groups.Add(group);
        transporter2.Groups.Add(group);
        group.Transporters.Add(transporter1);
        group.Transporters.Add(transporter2);

        var operatorSingle = new Operator("opX", null, null, null, null, null, 1, accountId);

        var deviceA = new Device("dA", 1, "sa", 1, null, transporter1.TransporterId, operatorSingle.OperatorId)
        {
            Transporter = transporter1,
            Operator = operatorSingle
        };
        transporter1.Devices.Add(deviceA);
        operatorSingle.Devices.Add(deviceA);

        var deviceB = new Device("dB", 2, "sb", 1, null, transporter2.TransporterId, operatorSingle.OperatorId)
        {
            Transporter = transporter2,
            Operator = operatorSingle
        };
        transporter2.Devices.Add(deviceB);
        operatorSingle.Devices.Add(deviceB);

        await context.Users.AddAsync(user);
        await context.Groups.AddAsync(group);
        await context.Transporters.AddRangeAsync(transporter1, transporter2);
        await context.Operators.AddAsync(operatorSingle);
        await context.Devices.AddRangeAsync(deviceA, deviceB);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext);

        // Act
        var result = await reader.GetOperatorsByUserAsync(userId, CancellationToken.None);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.First().OperatorId, Is.EqualTo(operatorSingle.OperatorId));
    }

    [Test]
    public async Task GetOperatorAsync_IncludesCredential()
    {
        var dbName = nameof(GetOperatorAsync_IncludesCredential);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);

        var accountId = Guid.NewGuid();
        var @operator = new Operator("opC", null, null, null, null, null, 1, accountId);
        var credential = new Credential("http://uri", "user", "pass", null, null, "salt", @operator.OperatorId);
        @operator.Credential = credential;

        await context.Operators.AddAsync(@operator);
        await context.Credentials.AddAsync(credential);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext);

        var result = await reader.GetOperatorAsync(@operator.OperatorId, CancellationToken.None);

        Assert.That(result.OperatorId, Is.EqualTo(@operator.OperatorId));
        Assert.That(result.Credential.HasValue, Is.True);
        Assert.That(result.Credential.Value.Username, Is.EqualTo(credential.Username));
    }

    [Test]
    public async Task GetOperatorsAsync_AppliesFilters()
    {
        var dbName = nameof(GetOperatorsAsync_AppliesFilters);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);

        var accountId = Guid.NewGuid();
        var otherAccount = Guid.NewGuid();

        var op1 = new Operator("op1", null, null, null, null, null, 1, accountId);
        var op2 = new Operator("op2", null, null, null, null, null, 1, otherAccount);

        await context.Operators.AddRangeAsync(op1, op2);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext);

        // filter by AccountId
        var filters = new Filters(new Dictionary<string, object> { { "AccountId", accountId } });
        var result = await reader.GetOperatorsAsync(filters, CancellationToken.None);

        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result.First().AccountId, Is.EqualTo(accountId));
    }

    [Test]
    public async Task GetOperatorByTransporterAsync_ReturnsOperator()
    {
        var dbName = nameof(GetOperatorByTransporterAsync_ReturnsOperator);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);

        var accountId = Guid.NewGuid();
        var transporter = new Transporter("t", 1);
        var @operator = new Operator("opT", null, null, null, null, null, 1, accountId);

        // Create a group and associate it with the transporter (required for accountId lookup)
        var group = new Group("testGroup", "desc", true, accountId);
        transporter.Groups.Add(group);
        group.Transporters.Add(transporter);

        var device = new Device("d", 1, "s", 1, null, transporter.TransporterId, @operator.OperatorId)
        {
            Transporter = transporter,
            Operator = @operator
        };
        transporter.Devices.Add(device);
        @operator.Devices.Add(device);

        await context.Transporters.AddAsync(transporter);
        await context.Operators.AddAsync(@operator);
        await context.Groups.AddAsync(group);
        await context.Devices.AddAsync(device);
        await context.SaveChangesAsync(CancellationToken.None);

        var reader = new OperatorReader(context as IApplicationDbContext);

        var result = await reader.GetOperatorByTransporterAsync(transporter.TransporterId, CancellationToken.None);

        Assert.That(result.OperatorId, Is.EqualTo(@operator.OperatorId));
    }
}
