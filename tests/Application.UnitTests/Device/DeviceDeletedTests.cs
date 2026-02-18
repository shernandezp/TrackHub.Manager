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

using TrackHub.Manager.Application.Device.Events;
using TrackHub.Manager.Domain.Interfaces;

namespace Application.UnitTests.Device;

[TestFixture]
public class DeviceDeletedTests
{
    private Mock<ITransporterWriter> _transporterWriterMock;
    private Mock<ITransporterGroupWriter> _transporterGroupWriterMock;
    private Mock<ITransporterPositionWriter> _transporterPositionWriterMock;
    private Mock<IDeviceReader> _deviceReaderMock;

    [SetUp]
    public void SetUp()
    {
        _transporterWriterMock = new Mock<ITransporterWriter>();
        _transporterGroupWriterMock = new Mock<ITransporterGroupWriter>();
        _transporterPositionWriterMock = new Mock<ITransporterPositionWriter>();
        _deviceReaderMock = new Mock<IDeviceReader>();
    }

    [Test]
    public async Task Handle_OtherDevicesExist_DoesNotDeleteTransporter()
    {
        // Arrange — another device still exists for this transporter
        var transporterId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        _deviceReaderMock.Setup(d => d.ExistDeviceAsync(transporterId, deviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new DeviceDeleted.Notification.EventHandler(
            _transporterWriterMock.Object,
            _transporterGroupWriterMock.Object,
            _transporterPositionWriterMock.Object,
            _deviceReaderMock.Object);

        // Act
        await handler.Handle(new DeviceDeleted.Notification(transporterId, deviceId), CancellationToken.None);

        // Assert — no cascade deletion should occur
        _transporterGroupWriterMock.Verify(w => w.DeleteTransporterGroupsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _transporterPositionWriterMock.Verify(w => w.DeleteTransporterPositionAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _transporterWriterMock.Verify(w => w.DeleteTransporterAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Handle_NoOtherDevicesExist_DeletesGroupsPositionsAndTransporter()
    {
        // Arrange — this was the last device
        var transporterId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();

        _deviceReaderMock.Setup(d => d.ExistDeviceAsync(transporterId, deviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = new DeviceDeleted.Notification.EventHandler(
            _transporterWriterMock.Object,
            _transporterGroupWriterMock.Object,
            _transporterPositionWriterMock.Object,
            _deviceReaderMock.Object);

        // Act
        await handler.Handle(new DeviceDeleted.Notification(transporterId, deviceId), CancellationToken.None);

        // Assert — full cascade: groups → positions → transporter
        _transporterGroupWriterMock.Verify(w => w.DeleteTransporterGroupsAsync(transporterId, It.IsAny<CancellationToken>()), Times.Once);
        _transporterPositionWriterMock.Verify(w => w.DeleteTransporterPositionAsync(transporterId, It.IsAny<CancellationToken>()), Times.Once);
        _transporterWriterMock.Verify(w => w.DeleteTransporterAsync(transporterId, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_NoOtherDevices_CascadeOrderIsCorrect()
    {
        // Arrange — verify order: groups first, then positions, then transporter
        var transporterId = Guid.NewGuid();
        var deviceId = Guid.NewGuid();
        var callOrder = new List<string>();

        _deviceReaderMock.Setup(d => d.ExistDeviceAsync(transporterId, deviceId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);
        _transporterGroupWriterMock.Setup(w => w.DeleteTransporterGroupsAsync(transporterId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("groups"))
            .Returns(Task.CompletedTask);
        _transporterPositionWriterMock.Setup(w => w.DeleteTransporterPositionAsync(transporterId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("positions"))
            .Returns(Task.CompletedTask);
        _transporterWriterMock.Setup(w => w.DeleteTransporterAsync(transporterId, It.IsAny<CancellationToken>()))
            .Callback(() => callOrder.Add("transporter"))
            .Returns(Task.CompletedTask);

        var handler = new DeviceDeleted.Notification.EventHandler(
            _transporterWriterMock.Object,
            _transporterGroupWriterMock.Object,
            _transporterPositionWriterMock.Object,
            _deviceReaderMock.Object);

        // Act
        await handler.Handle(new DeviceDeleted.Notification(transporterId, deviceId), CancellationToken.None);

        // Assert
        Assert.That(callOrder, Is.EqualTo(new[] { "groups", "positions", "transporter" }));
    }
}
