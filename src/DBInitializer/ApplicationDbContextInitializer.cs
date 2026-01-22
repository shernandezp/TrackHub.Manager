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

using Common.Domain.Constants;
using Microsoft.Extensions.Logging;
using TrackHub.Manager.Infrastructure.Entities;
using TrackHub.Manager.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Common.Domain.Enums;
using TransporterType = Common.Domain.Enums.TransporterType;

namespace DBInitializer;

internal class ApplicationDbContextInitializer(ILogger<ApplicationDbContextInitializer> logger, ApplicationDbContext context)
{
    public async Task InitializeAsync()
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default data
        // Seed, if necessary
        if (!context.Reports.Any())
        {
            var reportType = (short)ReportType.Basic;
            context.Reports.Add(new Report(Reports.TransportersInGeofence, "Transporters In Geofence", reportType, true));
            context.Reports.Add(new Report(Reports.PositionRecord, "Position Record", reportType, true));
            context.Reports.Add(new Report(Reports.LiveReport, "Live Report", reportType, true));
            await context.SaveChangesAsync();
        }
        if (!context.TransporterTypes.Any())
        {
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Aircraft, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Asset, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Bicycle, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Boat, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Car, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.CargoContainer, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Child, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.ConstructionVehicle, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.DeliveryVan, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Drone, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.ElderlyPerson, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.FleetVehicle, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.HeavyEquipment, true, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Livestock, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Motorcycle, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Package, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Person, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Pet, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.SchoolBus, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Scooter, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Taxi, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Tool, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Tractor, false, 10, 10, 120));
            context.TransporterTypes.Add(new TrackHub.Manager.Infrastructure.Entities.TransporterType((short)TransporterType.Truck, false, 10, 10, 120));
            await context.SaveChangesAsync();
        }
        if (!context.Accounts.Any())
        {
            var accountType = (short)AccountType.Personal;
            context.Accounts.Add(new Account("Master Acccount", "Master Account Description", accountType, true));
            await context.SaveChangesAsync();
        }
        if (!context.AccountSettings.Any())
        {
            var account = await context.Accounts.FirstAsync();
            context.AccountSettings.Add(new AccountSettings(account.AccountId));
            await context.SaveChangesAsync();
        }
        if (!context.Users.Any())
        {
            var account = await context.Accounts.FirstAsync();
            context.Users.Add(new User(Guid.NewGuid(), "Administrator", true, account.AccountId));
            await context.SaveChangesAsync();
        }
        if (!context.UserSettings.Any())
        {
            var user = await context.Users.FirstAsync();
            context.UserSettings.Add(new UserSettings(user.UserId));
            await context.SaveChangesAsync();
        }
    }
}
