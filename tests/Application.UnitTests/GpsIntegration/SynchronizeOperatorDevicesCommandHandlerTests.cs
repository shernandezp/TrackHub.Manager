using Microsoft.Extensions.Logging;
using Common.Domain.Enums;
using TrackHub.Manager.Application.GpsIntegration.Commands;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace Application.UnitTests.GpsIntegration;

[TestFixture]
public class SynchronizeOperatorDevicesCommandHandlerTests
{
    private Mock<IDeviceWriter> _deviceWriter = null!;
    private Mock<IDeviceReader> _deviceReader = null!;
    private Mock<ITransporterWriter> _transporterWriter = null!;
    private Mock<ITransporterDeviceAssignmentWriter> _assignmentWriter = null!;
    private Mock<IOperatorSyncRunWriter> _syncWriter = null!;
    private Mock<IOperatorWriter> _operatorWriter = null!;
    private Mock<IAlertEventWriter> _alertWriter = null!;

    [SetUp]
    public void SetUp()
    {
        _deviceWriter = new Mock<IDeviceWriter>();
        _deviceReader = new Mock<IDeviceReader>();
        _transporterWriter = new Mock<ITransporterWriter>();
        _assignmentWriter = new Mock<ITransporterDeviceAssignmentWriter>();
        _syncWriter = new Mock<IOperatorSyncRunWriter>();
        _operatorWriter = new Mock<IOperatorWriter>();
        _alertWriter = new Mock<IAlertEventWriter>();
        _deviceReader.Setup(x => x.GetDevicesByOperatorAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _deviceReader.Setup(x => x.FindDuplicateSerialsAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<IReadOnlyCollection<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        _syncWriter.Setup(x => x.RecordAsync(It.IsAny<OperatorSyncRunDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((OperatorSyncRunDto dto, CancellationToken _) => new OperatorSyncRunVm(
                Guid.NewGuid(),
                dto.AccountId,
                dto.OperatorId,
                dto.TriggerType,
                dto.Result,
                dto.StartedAt,
                dto.CompletedAt,
                dto.DevicesSeen,
                dto.DevicesAdded,
                dto.DevicesUpdated,
                dto.DevicesRemoved,
                dto.DevicesIgnored,
                dto.PositionsRead,
                dto.PositionsAccepted,
                dto.PositionsRejected,
                dto.ErrorCode,
                dto.ErrorMessage,
                dto.CorrelationId));
    }

    private SynchronizeOperatorDevicesCommandHandler CreateHandler() => new(
        _deviceWriter.Object,
        _deviceReader.Object,
        _transporterWriter.Object,
        _assignmentWriter.Object,
        _syncWriter.Object,
        _operatorWriter.Object,
        _alertWriter.Object,
        Mock.Of<ILogger<SynchronizeOperatorDevicesCommandHandler>>());

    [Test]
    public async Task Handle_NewDevices_AutoCreatesTransportersAndAssignmentsAndRecordsManualTrigger()
    {
        var accountId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var transporterId = Guid.NewGuid();
        var device = new DeviceDto(
            accountId,
            operatorId,
            "SER-1",
            "Device 1",
            101,
            "Truck 101",
            (short)DeviceType.OBDScanner,
            null,
            "hash",
            "ACTIVE");
        _deviceWriter.Setup(x => x.UpsertSynchronizedDeviceAsync(device, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeviceVm(
                deviceId,
                accountId,
                operatorId,
                device.Serial,
                device.Name,
                device.Identifier,
                device.ProviderDisplayName,
                DeviceType.OBDScanner,
                device.DeviceTypeId,
                device.Description,
                device.ProviderMetadataHash,
                device.ProviderStatus,
                DetectedStatus.New,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                null,
                null));
        _transporterWriter.Setup(x => x.CreateTransporterAsync(
                It.Is<TransporterDto>(dto =>
                    dto.AccountId == accountId
                    && dto.Name == "Truck 101"
                    && dto.TransporterTypeId == (short)TransporterType.FleetVehicle),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new TransporterVm(transporterId, "Truck 101", TransporterType.FleetVehicle, (short)TransporterType.FleetVehicle));
        _assignmentWriter.Setup(x => x.AssignAsync(It.IsAny<TransporterDeviceAssignmentDto>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((TransporterDeviceAssignmentDto dto, CancellationToken _) => new TransporterDeviceAssignmentVm(
                Guid.NewGuid(),
                dto.AccountId,
                dto.TransporterId,
                dto.DeviceId,
                DateTimeOffset.UtcNow,
                null,
                dto.Priority,
                dto.IsPrimary,
                AssignmentStatus.Active,
                dto.AssignmentReason,
                "ServiceClient",
                "syncworker_client"));

        var result = await CreateHandler().Handle(
            new SynchronizeOperatorDevicesCommand(accountId, operatorId, [device], "corr-1", "MANUAL"),
            CancellationToken.None);

        Assert.That(result.TriggerType, Is.EqualTo(SyncTriggerType.Manual));
        _assignmentWriter.Verify(x => x.AssignAsync(
            It.Is<TransporterDeviceAssignmentDto>(dto =>
                dto.AccountId == accountId
                && dto.TransporterId == transporterId
                && dto.DeviceId == deviceId
                && dto.IsPrimary
                && dto.AssignmentReason == "Initial provider sync"),
            It.IsAny<CancellationToken>()), Times.Once);
        _syncWriter.Verify(x => x.RecordAsync(
            It.Is<OperatorSyncRunDto>(dto =>
                dto.TriggerType == SyncTriggerType.Manual
                && dto.DevicesSeen == 1
                && dto.DevicesAdded == 1
                && dto.CorrelationId == "corr-1"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_AutoAssignDisabled_DoesNotCreateTransporterOrAssignment()
    {
        var accountId = Guid.NewGuid();
        var operatorId = Guid.NewGuid();
        var device = new DeviceDto(accountId, operatorId, "SER-1", "Device 1", 101, null, (short)DeviceType.OBDScanner, null, null, "ACTIVE");
        _deviceWriter.Setup(x => x.UpsertSynchronizedDeviceAsync(device, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new DeviceVm(
                Guid.NewGuid(),
                accountId,
                operatorId,
                device.Serial,
                device.Name,
                device.Identifier,
                device.ProviderDisplayName,
                DeviceType.OBDScanner,
                device.DeviceTypeId,
                device.Description,
                device.ProviderMetadataHash,
                device.ProviderStatus,
                DetectedStatus.New,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow,
                null,
                null));

        await CreateHandler().Handle(
            new SynchronizeOperatorDevicesCommand(accountId, operatorId, [device], "corr-2", AutoAssignNewDevices: false),
            CancellationToken.None);

        _transporterWriter.Verify(x => x.CreateTransporterAsync(It.IsAny<TransporterDto>(), It.IsAny<CancellationToken>()), Times.Never);
        _assignmentWriter.Verify(x => x.AssignAsync(It.IsAny<TransporterDeviceAssignmentDto>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
