using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Common.Domain.Enums;
using TrackHub.Manager.Application.GpsIntegration.Commands;
using TrackHub.Manager.Domain.Interfaces;

namespace Application.UnitTests.GpsIntegration;

[TestFixture]
public class TriggerOperatorDeviceSyncCommandHandlerTests
{
    private static OperatorVm Operator(Guid operatorId, Guid accountId, DateTimeOffset? lastManualSyncAt = null)
        => new(
            operatorId,
            "Provider",
            null,
            null,
            null,
            null,
            null,
            ProtocolType.Traccar,
            2,
            accountId,
            DateTimeOffset.UtcNow,
            null,
            true,
            60,
            OperatorHealthStatus.Unknown,
            null,
            null,
            lastManualSyncAt,
            null,
            null,
            null,
            null,
            null);

    [Test]
    public async Task Handle_ReturnsDispatcherResultWithoutThrowingWhenRouterRejectsReset()
    {
        var accountId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        var operatorReader = new Mock<IOperatorReader>();
        var dispatcher = new Mock<ISyncDispatcher>();
        var configuration = new Mock<IConfiguration>();
        operatorReader.Setup(x => x.GetOperatorAsync(operatorId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Operator(operatorId, accountId));
        dispatcher.Setup(x => x.DispatchManualSyncAsync(accountId, operatorId, true, true, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        configuration.Setup(x => x["GpsIntegration:ManualSyncMinIntervalSeconds"])
            .Returns("0");
        var handler = new TriggerOperatorDeviceSyncCommandHandler(
            operatorReader.Object,
            dispatcher.Object,
            configuration.Object,
            Mock.Of<ILogger<TriggerOperatorDeviceSyncCommandHandler>>());

        var result = await handler.Handle(new TriggerOperatorDeviceSyncCommand(operatorId, ResetDeviceCatalog: true, AutoAssignNewDevices: true), CancellationToken.None);

        Assert.That(result, Is.False);
        dispatcher.Verify(x => x.DispatchManualSyncAsync(accountId, operatorId, true, true, It.IsAny<CancellationToken>()), Times.Once);
    }
}
