using TrackHub.Manager.Domain.Enums;
using TrackHub.Manager.Infrastructure;
using TrackHub.Manager.Infrastructure.Interfaces;
using TrackHub.Manager.Infrastructure.ManagerDB.Writers;

namespace Infrastructure.UnitTests;

[TestFixture]
public class OperatorWriterTests
{
    private static ApplicationDbContext NewContext(string name)
        => new(new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(name).Options);

    [Test]
    public async Task SetEnabledAsync_EnablingDisabledOperator_ResetsHealthToUnknown()
    {
        var accountId = Guid.NewGuid();
        await using var context = NewContext(nameof(SetEnabledAsync_EnablingDisabledOperator_ResetsHealthToUnknown));
        var op = new Operator("gps", null, null, null, null, null, 1, accountId)
        {
            Enabled = false,
            HealthStatus = (int)OperatorHealthStatus.Disabled
        };
        await context.Operators.AddAsync(op);
        await context.SaveChangesAsync();

        var writer = new OperatorWriter(context as IApplicationDbContext);

        await writer.SetEnabledAsync(op.OperatorId, true, CancellationToken.None);

        var saved = await context.Operators.SingleAsync(x => x.OperatorId == op.OperatorId);
        Assert.Multiple(() =>
        {
            Assert.That(saved.Enabled, Is.True);
            Assert.That(saved.HealthStatus, Is.EqualTo((int)OperatorHealthStatus.Unknown));
        });
    }

    [Test]
    public async Task SetEnabledAsync_DisablingOperator_SetsHealthToDisabled()
    {
        var accountId = Guid.NewGuid();
        await using var context = NewContext(nameof(SetEnabledAsync_DisablingOperator_SetsHealthToDisabled));
        var op = new Operator("gps", null, null, null, null, null, 1, accountId)
        {
            Enabled = true,
            HealthStatus = (int)OperatorHealthStatus.Healthy
        };
        await context.Operators.AddAsync(op);
        await context.SaveChangesAsync();

        var writer = new OperatorWriter(context as IApplicationDbContext);

        await writer.SetEnabledAsync(op.OperatorId, false, CancellationToken.None);

        var saved = await context.Operators.SingleAsync(x => x.OperatorId == op.OperatorId);
        Assert.Multiple(() =>
        {
            Assert.That(saved.Enabled, Is.False);
            Assert.That(saved.HealthStatus, Is.EqualTo((int)OperatorHealthStatus.Disabled));
        });
    }
}
