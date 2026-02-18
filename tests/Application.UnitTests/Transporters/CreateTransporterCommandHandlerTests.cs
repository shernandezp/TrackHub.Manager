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

using Common.Domain.Enums;
using TrackHub.Manager.Application.Transporters.Commands.Create;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Records;

namespace Application.UnitTests.Transporters;

[TestFixture]
public class CreateTransporterCommandHandlerTests
{
    private Mock<ITransporterWriter> _writerMock;

    [SetUp]
    public void SetUp()
    {
        _writerMock = new Mock<ITransporterWriter>();
    }

    [Test]
    public async Task Handle_ValidCommand_DelegatesToWriter()
    {
        // Arrange
        var dto = new TransporterDto("Truck-001", 1);
        var expectedVm = new TransporterVm(Guid.NewGuid(), "Truck-001", TransporterType.Truck, 1);
        _writerMock.Setup(w => w.CreateTransporterAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedVm);

        var handler = new CreateTransporterCommandHandler(_writerMock.Object);

        // Act
        var result = await handler.Handle(new CreateTransporterCommand(dto), CancellationToken.None);

        // Assert
        Assert.That(result, Is.EqualTo(expectedVm));
        _writerMock.Verify(w => w.CreateTransporterAsync(dto, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_ReturnsTransporterVm_WithCorrectTypeId()
    {
        // Arrange
        var dto = new TransporterDto("Asset-X", 3);
        var expectedVm = new TransporterVm(Guid.NewGuid(), "Asset-X", TransporterType.Bicycle, 3);
        _writerMock.Setup(w => w.CreateTransporterAsync(dto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedVm);

        var handler = new CreateTransporterCommandHandler(_writerMock.Object);

        // Act
        var result = await handler.Handle(new CreateTransporterCommand(dto), CancellationToken.None);

        // Assert
        Assert.That(result.TransporterTypeId, Is.EqualTo(3));
        Assert.That(result.Name, Is.EqualTo("Asset-X"));
    }
}
