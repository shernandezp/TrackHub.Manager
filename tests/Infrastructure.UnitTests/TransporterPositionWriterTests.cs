// Copyright (c) 2025 Sergio Hernandez. All rights reserved.
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

using TrackHub.Manager.Domain.Models;
using TrackHub.Manager.Domain.Records;
using TrackHub.Manager.Infrastructure;
using TrackHub.Manager.Infrastructure.Interfaces;
using TrackHub.Manager.Infrastructure.ManagerDB.Writers;

namespace Infrastructure.UnitTests;

[TestFixture]
public class TransporterPositionWriterTests
{
    [Test]
    public async Task BulkTransporterPositionAsync_AddsNewPositions_WhenNoneExist()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_AddsNewPositions_WhenNoneExist);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var transporterId1 = Guid.NewGuid();
        var transporterId2 = Guid.NewGuid();

        var positions = new List<TransporterPositionDto>
        {
            CreatePositionDto(transporterId1, 10.0, 20.0),
            CreatePositionDto(transporterId2, 30.0, 40.0)
        };

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.ToListAsync();
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.Any(p => p.TransporterId == transporterId1 && p.Latitude == 10.0), Is.True);
        Assert.That(result.Any(p => p.TransporterId == transporterId2 && p.Latitude == 30.0), Is.True);
    }

    [Test]
    public async Task BulkTransporterPositionAsync_UpdatesExistingPositions()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_UpdatesExistingPositions);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var transporterId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        // Add existing position
        var existingPosition = new TransporterPosition(
            transporterId, null, 10.0, 20.0, null, DateTime.UtcNow,
            TimeSpan.Zero, 50.0, null, null, null, "OldCity", null, null, null);
        await context.TransporterPositions.AddAsync(existingPosition);
        await context.SaveChangesAsync(CancellationToken.None);

        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var updatedPositions = new List<TransporterPositionDto>
        {
            CreatePositionDto(transporterId, 99.0, 99.0, speed: 100.0, city: "NewCity")
        };

        // Act
        await writer.BulkTransporterPositionAsync(updatedPositions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.ToListAsync();
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Latitude, Is.EqualTo(99.0));
        Assert.That(result[0].Longitude, Is.EqualTo(99.0));
        Assert.That(result[0].Speed, Is.EqualTo(100.0));
        Assert.That(result[0].City, Is.EqualTo("NewCity"));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_HandlesDuplicateTransporters_KeepsLast()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_HandlesDuplicateTransporters_KeepsLast);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var transporterId = Guid.NewGuid();

        // Multiple positions for same transporter
        var positions = new List<TransporterPositionDto>
        {
            CreatePositionDto(transporterId, 10.0, 20.0, speed: 50.0),
            CreatePositionDto(transporterId, 30.0, 40.0, speed: 75.0),
            CreatePositionDto(transporterId, 50.0, 60.0, speed: 100.0) // Last one
        };

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.ToListAsync();
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.That(result[0].Latitude, Is.EqualTo(50.0));
        Assert.That(result[0].Speed, Is.EqualTo(100.0));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_WithAttributes_MapsCorrectly()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_WithAttributes_MapsCorrectly);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var transporterId = Guid.NewGuid();
        var attributes = new AttributesDto(true, 12, 5000.5, 1000.0, 25.5);

        var positions = new List<TransporterPositionDto>
        {
            CreatePositionDto(transporterId, 10.0, 20.0, attributes: attributes)
        };

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.FirstAsync();
        Assert.That(result.Attributes, Is.Not.Null);
        Assert.That(result.Attributes!.Value.Ignition, Is.True);
        Assert.That(result.Attributes!.Value.Satellites, Is.EqualTo(12));
        Assert.That(result.Attributes!.Value.Mileage, Is.EqualTo(5000.5));
        Assert.That(result.Attributes!.Value.Hourmeter, Is.EqualTo(1000.0));
        Assert.That(result.Attributes!.Value.Temperature, Is.EqualTo(25.5));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_WithNullAttributes_HandlesCorrectly()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_WithNullAttributes_HandlesCorrectly);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var transporterId = Guid.NewGuid();

        var positions = new List<TransporterPositionDto>
        {
            CreatePositionDto(transporterId, 10.0, 20.0, attributes: null)
        };

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.FirstAsync();
        Assert.That(result.Attributes, Is.Null);
    }

    [Test]
    public async Task BulkTransporterPositionAsync_MixedAddAndUpdate_HandlesCorrectly()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_MixedAddAndUpdate_HandlesCorrectly);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var existingTransporterId = Guid.NewGuid();
        var newTransporterId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        // Add existing position
        var existingPosition = new TransporterPosition(
            existingTransporterId, null, 1.0, 2.0, null, DateTime.UtcNow,
            TimeSpan.Zero, 10.0, null, null, null, null, null, null, null);
        await context.TransporterPositions.AddAsync(existingPosition);
        await context.SaveChangesAsync(CancellationToken.None);

        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var positions = new List<TransporterPositionDto>
        {
            CreatePositionDto(existingTransporterId, 99.0, 99.0), // Update
            CreatePositionDto(newTransporterId, 11.0, 22.0)       // New
        };

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.ToListAsync();
        Assert.That(result, Has.Count.EqualTo(2));

        var updated = result.First(p => p.TransporterId == existingTransporterId);
        Assert.That(updated.Latitude, Is.EqualTo(99.0));

        var added = result.First(p => p.TransporterId == newTransporterId);
        Assert.That(added.Latitude, Is.EqualTo(11.0));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_PreservesTransporterPositionId_OnUpdate()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_PreservesTransporterPositionId_OnUpdate);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var transporterId = Guid.NewGuid();
        var originalPositionId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        // Add existing position with specific ID
        var existingPosition = new TransporterPosition(
            transporterId, null, 10.0, 20.0, null, DateTime.UtcNow,
            TimeSpan.Zero, 50.0, null, null, null, null, null, null, null)
        {
            TransporterPositionId = originalPositionId
        };
        await context.TransporterPositions.AddAsync(existingPosition);
        await context.SaveChangesAsync(CancellationToken.None);

        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var updatedPositions = new List<TransporterPositionDto>
        {
            CreatePositionDto(transporterId, 99.0, 99.0)
        };

        // Act
        await writer.BulkTransporterPositionAsync(updatedPositions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.FirstAsync();
        Assert.That(result.TransporterPositionId, Is.EqualTo(originalPositionId));
        Assert.That(result.Latitude, Is.EqualTo(99.0));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_EmptyCollection_DoesNotThrow()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_EmptyCollection_DoesNotThrow);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () =>
            await writer.BulkTransporterPositionAsync([], CancellationToken.None));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_LargeBatch_HandlesEfficiently()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_LargeBatch_HandlesEfficiently);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        // Create 100 positions
        var positions = Enumerable.Range(0, 100)
            .Select(_ => CreatePositionDto(Guid.NewGuid(), Random.Shared.NextDouble() * 90, Random.Shared.NextDouble() * 180))
            .ToList();

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.ToListAsync();
        Assert.That(result, Has.Count.EqualTo(100));
    }

    [Test]
    public async Task BulkTransporterPositionAsync_DateTimeOffset_MapsToUtcAndOffset()
    {
        // Arrange
        var dbName = nameof(BulkTransporterPositionAsync_DateTimeOffset_MapsToUtcAndOffset);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var transporterId = Guid.NewGuid();
        var deviceDateTime = new DateTimeOffset(2025, 6, 15, 10, 30, 0, TimeSpan.FromHours(-5));

        var positions = new List<TransporterPositionDto>
        {
            new(transporterId, null, 10.0, 20.0, null, deviceDateTime, 0, null, null, null, null, null, null, null)
        };

        // Act
        await writer.BulkTransporterPositionAsync(positions, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.FirstAsync();
        Assert.That(result.DateTime, Is.EqualTo(deviceDateTime.UtcDateTime));
        Assert.That(result.Offset, Is.EqualTo(TimeSpan.FromHours(-5)));
    }

    [Test]
    public async Task UpdateTransporterPositionAsync_UpdatesAllFields()
    {
        // Arrange
        var dbName = nameof(UpdateTransporterPositionAsync_UpdatesAllFields);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var transporterId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        var existingPosition = new TransporterPosition(
            transporterId, null, 10.0, 20.0, 100.0, DateTime.UtcNow,
            TimeSpan.Zero, 50.0, 90.0, 1, "OldAddress", "OldCity", "OldState", "OldCountry", null);
        await context.TransporterPositions.AddAsync(existingPosition);
        await context.SaveChangesAsync(CancellationToken.None);

        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        var geometryId = Guid.NewGuid();
        var newDateTime = new DateTimeOffset(2025, 1, 1, 12, 0, 0, TimeSpan.FromHours(2));
        var attributes = new AttributesDto(true, 10, 500.0, 100.0, 20.0);

        var updateDto = new TransporterPositionDto(
            transporterId, geometryId, 99.0, 99.0, 200.0, newDateTime, 100.0, 180.0, 2,
            "NewAddress", "NewCity", "NewState", "NewCountry", attributes);

        // Act
        await writer.UpdateTransporterPositionAsync(updateDto, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.FirstAsync();
        Assert.That(result.GeometryId, Is.EqualTo(geometryId));
        Assert.That(result.Latitude, Is.EqualTo(99.0));
        Assert.That(result.Longitude, Is.EqualTo(99.0));
        Assert.That(result.Altitude, Is.EqualTo(200.0));
        Assert.That(result.Speed, Is.EqualTo(100.0));
        Assert.That(result.Course, Is.EqualTo(180.0));
        Assert.That(result.EventId, Is.EqualTo(2));
        Assert.That(result.Address, Is.EqualTo("NewAddress"));
        Assert.That(result.City, Is.EqualTo("NewCity"));
        Assert.That(result.State, Is.EqualTo("NewState"));
        Assert.That(result.Country, Is.EqualTo("NewCountry"));
        Assert.That(result.Attributes, Is.Not.Null);
    }

    [Test]
    public async Task DeleteTransporterPositionAsync_RemovesPosition()
    {
        // Arrange
        var dbName = nameof(DeleteTransporterPositionAsync_RemovesPosition);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        var transporterId = Guid.NewGuid();

        await using var context = new ApplicationDbContext(options);

        var existingPosition = new TransporterPosition(
            transporterId, null, 10.0, 20.0, null, DateTime.UtcNow,
            TimeSpan.Zero, 50.0, null, null, null, null, null, null, null);
        await context.TransporterPositions.AddAsync(existingPosition);
        await context.SaveChangesAsync(CancellationToken.None);

        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        // Act
        await writer.DeleteTransporterPositionAsync(transporterId, CancellationToken.None);

        // Assert
        var result = await context.TransporterPositions.ToListAsync();
        Assert.That(result, Is.Empty);
    }

    [Test]
    public async Task DeleteTransporterPositionAsync_NonExistent_DoesNotThrow()
    {
        // Arrange
        var dbName = nameof(DeleteTransporterPositionAsync_NonExistent_DoesNotThrow);
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var context = new ApplicationDbContext(options);
        var writer = new TransporterPositionWriter(context as IApplicationDbContext);

        // Act & Assert
        Assert.DoesNotThrowAsync(async () =>
            await writer.DeleteTransporterPositionAsync(Guid.NewGuid(), CancellationToken.None));
    }

    private static TransporterPositionDto CreatePositionDto(
        Guid transporterId,
        double latitude,
        double longitude,
        double speed = 0.0,
        string? city = null,
        AttributesDto? attributes = null)
    {
        return new TransporterPositionDto(
            transporterId,
            null,
            latitude,
            longitude,
            null,
            DateTimeOffset.UtcNow,
            speed,
            null,
            null,
            null,
            city,
            null,
            null,
            attributes);
    }
}
