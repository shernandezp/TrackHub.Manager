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

using Common.Application.Attributes;
using Common.Domain.Constants;
using TrackHub.Manager.Application.TransporterPosition.Commands.Create;
using TrackHub.Manager.Domain.Interfaces;
using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;

namespace Application.UnitTests.TransporterPositions;

[TestFixture]
public class BulkTransporterPositionCommandHandlerTests
{
    private Mock<ITransporterPositionWriter> _writerMock = null!;
    private CreateTransporterCommandHandler _handler = null!;

    [SetUp]
    public void SetUp()
    {
        _writerMock = new Mock<ITransporterPositionWriter>();
        _handler = new CreateTransporterCommandHandler(_writerMock.Object);
    }

    [Test]
    public void BulkTransporterPositionCommand_DoesNotRequireGpsIntegrationFeature()
    {
        var attributes = Attribute.GetCustomAttributes(typeof(BulkTransporterPositionCommand), typeof(RequireFeatureAttribute))
            .Cast<RequireFeatureAttribute>()
            .ToArray();

        Assert.That(
            attributes.Any(attribute => attribute.FeatureKey == FeatureKeys.GpsIntegration),
            Is.False,
            "Latest-position upsert is core TrackHub behavior and must not be blocked by the optional GPS integration module.");
    }

    [Test]
    public async Task Handle_DelegatesToWriter_PreservingDistinctTransporters()
    {
        var positions = new[]
        {
            NewPosition(Guid.NewGuid(), DateTimeOffset.UtcNow),
            NewPosition(Guid.NewGuid(), DateTimeOffset.UtcNow.AddSeconds(-30))
        };

        IEnumerable<TransporterPositionDto>? captured = null;
        _writerMock.Setup(w => w.BulkTransporterPositionAsync(
                It.IsAny<IEnumerable<TransporterPositionDto>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<TransporterPositionDto>, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        await _handler.Handle(new BulkTransporterPositionCommand(positions), CancellationToken.None);

        Assert.That(captured, Is.Not.Null);
        Assert.That(captured!.Count(), Is.EqualTo(2));
        _writerMock.Verify(w => w.BulkTransporterPositionAsync(
            It.IsAny<IEnumerable<TransporterPositionDto>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_KeepsOnlyNewestPositionPerTransporter()
    {
        var transporterId = Guid.NewGuid();
        var otherTransporterId = Guid.NewGuid();
        var older = NewPosition(transporterId, DateTimeOffset.UtcNow.AddMinutes(-5), latitude: 10.0);
        var middle = NewPosition(transporterId, DateTimeOffset.UtcNow.AddMinutes(-1), latitude: 10.5);
        var newest = NewPosition(transporterId, DateTimeOffset.UtcNow, latitude: 11.0);
        var other = NewPosition(otherTransporterId, DateTimeOffset.UtcNow.AddMinutes(-2), latitude: 42.0);

        IEnumerable<TransporterPositionDto>? captured = null;
        _writerMock.Setup(w => w.BulkTransporterPositionAsync(
                It.IsAny<IEnumerable<TransporterPositionDto>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<TransporterPositionDto>, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        await _handler.Handle(
            new BulkTransporterPositionCommand([older, newest, middle, other]),
            CancellationToken.None);

        Assert.That(captured, Is.Not.Null);
        var list = captured!.ToList();
        Assert.That(list, Has.Count.EqualTo(2));
        var winner = list.Single(p => p.TransporterId == transporterId);
        Assert.That(winner.Latitude, Is.EqualTo(11.0), "Newest device report must win for the same transporter.");
        Assert.That(list.Any(p => p.TransporterId == otherTransporterId), Is.True);
    }

    [Test]
    public async Task Handle_EmptyBatch_StillCallsWriterWithEmptySequence()
    {
        IEnumerable<TransporterPositionDto>? captured = null;
        _writerMock.Setup(w => w.BulkTransporterPositionAsync(
                It.IsAny<IEnumerable<TransporterPositionDto>>(), It.IsAny<CancellationToken>()))
            .Callback<IEnumerable<TransporterPositionDto>, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        await _handler.Handle(new BulkTransporterPositionCommand([]), CancellationToken.None);

        Assert.That(captured, Is.Not.Null);
        Assert.That(captured!, Is.Empty);
    }

    private static TransporterPositionDto NewPosition(Guid transporterId, DateTimeOffset deviceDateTime, double latitude = 0.0) => new(
        TransporterId: transporterId,
        GeometryId: null,
        Latitude: latitude,
        Longitude: 0.0,
        Altitude: null,
        DeviceDateTime: deviceDateTime,
        Speed: 0.0,
        Course: null,
        EventId: null,
        Address: null,
        City: null,
        State: null,
        Country: null,
        Attributes: null);
}
