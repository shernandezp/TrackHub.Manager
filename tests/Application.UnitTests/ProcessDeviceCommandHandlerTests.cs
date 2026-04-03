// Copyright (c) 2026 Sergio Hernandez. All rights reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License").
//  You may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

using TrackHub.Manager.Application.Device.Commands.Process;
using TrackHub.Manager.Application.Device.Events;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace Application.UnitTests;

[TestFixture]
public class ProcessDeviceCommandHandlerTests
{
    private Mock<IPublisher> _publisherMock;
    private Mock<IDeviceReader> _deviceReaderMock;
    private Mock<IOperatorReader> _operatorReaderMock;
    private Mock<ITransporterWriter> _transporterWriterMock;
    private Mock<ITransporterReader> _transporterReaderMock;

    private ProcessDeviceCommandHandler _handler;
    private Guid _accountId;

    [SetUp]
    public void Setup()
    {
        _publisherMock = new Mock<IPublisher>();
        _deviceReaderMock = new Mock<IDeviceReader>();
        _operatorReaderMock = new Mock<IOperatorReader>();
        _transporterWriterMock = new Mock<ITransporterWriter>();
        _transporterReaderMock = new Mock<ITransporterReader>();
        _accountId = Guid.NewGuid();

        _handler = new ProcessDeviceCommandHandler(
            _publisherMock.Object,
            _deviceReaderMock.Object,
            _operatorReaderMock.Object,
            _transporterWriterMock.Object,
            _transporterReaderMock.Object);
    }

    [Test]
    public async Task Handle_WhenDeviceDoesNotExist_ShouldCreateDeviceWithAccountId()
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

        var operatorVm = new OperatorVm
        {
            OperatorId = operatorId,
            Name = "TestOp",
            AccountId = _accountId,
            ProtocolTypeId = 1
        };
        _operatorReaderMock.Setup(x => x.GetOperatorAsync(operatorId, CancellationToken.None))
            .ReturnsAsync(operatorVm);

        _deviceReaderMock.Setup(x => x.GetDeviceAsync(processDevice.Serial, operatorId, CancellationToken.None))
            .ReturnsAsync(default(DeviceVm));

        _transporterReaderMock.Setup(x => x.GetTransporterAsync(processDevice.Name, CancellationToken.None))
            .ReturnsAsync(default(TransporterVm));

        _transporterWriterMock.Setup(x => x.CreateTransporterAsync(
                It.Is<TransporterDto>(d => d.AccountId == _accountId), CancellationToken.None))
            .ReturnsAsync(new TransporterVm { TransporterId = Guid.NewGuid() });

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.That(result, Is.True);
        _operatorReaderMock.Verify(x => x.GetOperatorAsync(operatorId, CancellationToken.None), Times.Once);
        _transporterWriterMock.Verify(x => x.CreateTransporterAsync(
            It.Is<TransporterDto>(d => d.AccountId == _accountId), CancellationToken.None), Times.Once);
        _publisherMock.Verify(x => x.Publish(
            It.Is<CreateDevice.Notification>(n => n.Device.AccountId == _accountId),
            CancellationToken.None), Times.Once);
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

        var operatorVm = new OperatorVm
        {
            OperatorId = operatorId,
            Name = "TestOp",
            AccountId = _accountId,
            ProtocolTypeId = 1
        };
        _operatorReaderMock.Setup(x => x.GetOperatorAsync(operatorId, CancellationToken.None))
            .ReturnsAsync(operatorVm);

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
        Assert.That(result, Is.True);
        _operatorReaderMock.Verify(x => x.GetOperatorAsync(operatorId, CancellationToken.None), Times.Once);
        _publisherMock.Verify(x => x.Publish(It.IsAny<UpdateDevice.Notification>(), CancellationToken.None), Times.Once);
    }

    [Test]
    public async Task Handle_WhenTransporterExists_ShouldNotCreateNewTransporter()
    {
        // Arrange
        var processDevice = new ProcessDeviceDto
        {
            Serial = "SER-001",
            Name = "ExistingTransporter",
            TransporterTypeId = 2
        };
        var operatorId = Guid.NewGuid();
        var existingTransporterId = Guid.NewGuid();
        var request = new ProcessDeviceCommand(processDevice, operatorId);

        var operatorVm = new OperatorVm
        {
            OperatorId = operatorId,
            Name = "TestOp",
            AccountId = _accountId,
            ProtocolTypeId = 1
        };
        _operatorReaderMock.Setup(x => x.GetOperatorAsync(operatorId, CancellationToken.None))
            .ReturnsAsync(operatorVm);

        _transporterReaderMock.Setup(x => x.GetTransporterAsync(processDevice.Name, CancellationToken.None))
            .ReturnsAsync(new TransporterVm { TransporterId = existingTransporterId });

        _deviceReaderMock.Setup(x => x.GetDeviceAsync(processDevice.Serial, operatorId, CancellationToken.None))
            .ReturnsAsync(default(DeviceVm));

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        _transporterWriterMock.Verify(x => x.CreateTransporterAsync(
            It.IsAny<TransporterDto>(), It.IsAny<CancellationToken>()), Times.Never);
        _publisherMock.Verify(x => x.Publish(
            It.Is<CreateDevice.Notification>(n =>
                n.Device.TransporterId == existingTransporterId &&
                n.Device.AccountId == _accountId),
            CancellationToken.None), Times.Once);
    }
}
