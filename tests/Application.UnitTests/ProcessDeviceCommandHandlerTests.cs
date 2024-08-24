using TrackHub.Manager.Application.Device.Commands.Process;
using TrackHub.Manager.Application.Device.Events;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace Application.UnitTests;

[TestFixture]
public class ProcessDeviceCommandHandlerTests
{
    private Mock<IPublisher> _publisherMock;
    private Mock<IDeviceReader> _deviceReaderMock;
    private Mock<ITransporterWriter> _transporterWriterMock;
    private Mock<ITransporterReader> _transporterReaderMock;

    private ProcessDeviceCommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _publisherMock = new Mock<IPublisher>();
        _deviceReaderMock = new Mock<IDeviceReader>();
        _transporterWriterMock = new Mock<ITransporterWriter>();
        _transporterReaderMock = new Mock<ITransporterReader>();

        _handler = new ProcessDeviceCommandHandler(
            _publisherMock.Object,
            _deviceReaderMock.Object,
            _transporterWriterMock.Object,
            _transporterReaderMock.Object);
    }

    [Test]
    public async Task Handle_WhenDeviceDoesNotExist_ShouldCreateDevice()
    {
        // Arrange
        var processDevice = new ProcessDeviceDto
        {
            Serial = "123456",
            Name = "Device1",
            TransporterTypeId = 1
        };
        var operatorId = Guid.NewGuid();

        var request = new ProcessDeviceCommand(processDevice, operatorId);

        _deviceReaderMock.Setup(x => x.GetDeviceAsync(processDevice.Serial, operatorId, CancellationToken.None))
            .ReturnsAsync(default(DeviceVm));

        _transporterReaderMock.Setup(x => x.GetTransporterAsync(processDevice.Name, CancellationToken.None))
            .ReturnsAsync(default(TransporterVm));

        _transporterWriterMock.Setup(x => x.CreateTransporterAsync(It.IsAny<TransporterDto>(), CancellationToken.None))
            .ReturnsAsync(new TransporterVm { TransporterId = Guid.NewGuid() });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _publisherMock.Verify(x => x.Publish(It.IsAny<CreateDevice.Notification>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Handle_WhenDeviceExists_ShouldUpdateDevice()
    {
        // Arrange
        var processDevice = new ProcessDeviceDto
        {
            Serial = "123456",
            Name = "Device1",
            TransporterTypeId = 1
        };
        var operatorId = Guid.NewGuid();

        var request = new ProcessDeviceCommand(processDevice, operatorId);

        var existingDevice = new DeviceVm
        {
            Name = "Device1",
            Identifier = 1,
            DeviceTypeId = 1,
            Description = "Device 1",
            TransporterId = Guid.NewGuid(),
        };

        _deviceReaderMock.Setup(x => x.GetDeviceAsync(processDevice.Serial, operatorId, CancellationToken.None))
            .ReturnsAsync(existingDevice);

        _transporterReaderMock.Setup(x => x.GetTransporterAsync(processDevice.Name, CancellationToken.None))
            .ReturnsAsync(new TransporterVm { TransporterId = Guid.NewGuid() });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        _publisherMock.Verify(x => x.Publish(It.IsAny<UpdateDevice.Notification>(), CancellationToken.None), Times.Once);
    }
}
