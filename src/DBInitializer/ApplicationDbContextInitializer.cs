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
    // Canonical catalog of all backend factory reports with their governance metadata.
    // Category grouping, RequiredFeatureKey gating (null = global), ManagerOnly role gating and
    // SupportsPdf format support. Common report-code constants where they exist; the Reporting-local
    // document/admin codes are string literals (they never became Common constants).
    private static readonly (string Code, string Description, string Category, string? RequiredFeatureKey, bool ManagerOnly, bool SupportsPdf, int SortOrder)[] ReportCatalog =
    [
        // Operations (global + geofencing-gated)
        (Reports.LiveReport, "Live Report", "Operations", null, false, false, 10),
        (Reports.PositionRecord, "Position Record", "Operations", null, false, false, 20),
        (Reports.TransportersInGeofence, "Transporters In Geofence", "Operations", FeatureKeys.Geofencing, false, false, 30),
        (Reports.GeofenceEvents, "Geofence Events", "Operations", FeatureKeys.Geofencing, false, false, 40),

        // GPS integration
        (Reports.GpsProviderHealthSummary, "GPS Provider Health Summary", "Gps", FeatureKeys.GpsIntegration, true, true, 10),
        (Reports.GpsProviderSyncHistory, "GPS Provider Sync History", "Gps", FeatureKeys.GpsIntegration, false, false, 20),
        (Reports.GpsSyncStatistics, "GPS Sync Statistics", "Gps", FeatureKeys.GpsIntegration, false, false, 30),
        (Reports.GpsSynchronizedDeviceInventory, "GPS Synchronized Device Inventory", "Gps", FeatureKeys.GpsIntegration, false, false, 40),
        (Reports.GpsRecentlyAddedDevices, "GPS Recently Added Devices", "Gps", FeatureKeys.GpsIntegration, false, false, 50),
        (Reports.GpsUnassignedDevices, "GPS Unassigned Devices", "Gps", FeatureKeys.GpsIntegration, false, false, 60),
        (Reports.GpsIgnoredDevices, "GPS Ignored Devices", "Gps", FeatureKeys.GpsIntegration, false, false, 70),
        (Reports.GpsAssignmentHistory, "GPS Assignment History", "Gps", FeatureKeys.GpsIntegration, false, false, 80),
        (Reports.GpsLatestPositionFreshness, "GPS Latest Position Freshness", "Gps", FeatureKeys.GpsIntegration, false, false, 90),
        (Reports.GpsPositionHistory, "GPS Position History", "Gps", FeatureKeys.GpsPositionHistory, false, false, 100),

        // Documents — Reporting-local codes
        ("documents-expiring", "Documents expiring within a window", "Documents", FeatureKeys.Documents, false, true, 10),
        ("documents-missing-required", "Transporters missing required documents", "Documents", FeatureKeys.Documents, false, true, 20),
        ("documents-share-activity", "Document share activity", "Documents", FeatureKeys.Documents, false, false, 30),
        ("documents-upload-volume", "Document upload volume", "Documents", FeatureKeys.Documents, false, false, 40),

        // Administration (global + manager-only) — Reporting-local codes
        ("accounts-by-status", "Accounts by lifecycle status", "Administration", null, true, true, 10),
        ("feature-enablement-matrix", "Feature enablement matrix across accounts", "Administration", null, true, true, 20),
        ("group-membership-export", "Group membership export", "Administration", null, true, false, 30),
    ];

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
        // Report catalog: idempotent per-code upsert that runs every start. New rows are
        // inserted Active; existing rows get their Description + governance metadata refreshed in place,
        // but Active and Type are never overwritten (admins may have disabled a report).
        //
        // DESIGN NOTE — code is the source of truth for seeded catalog metadata. Because this refresh runs
        // on every start, a SuperAdministrator's UpdateReportCommand edits to the seeded fields
        // (Description/Category/RequiredFeatureKey/ManagerOnly/SupportsPdf/SortOrder) are TRANSIENT: they
        // revert to these code values on the next restart. Only Active (enable/disable a report) persists
        // across restarts. This is intentional — the governed catalog's shape ships with the deployment;
        // UpdateReport is for toggling availability, not for permanently re-authoring seeded metadata.
        var reportType = (short)ReportType.Basic;
        var existingReports = await context.Reports.AsTracking().ToListAsync();
        var reportsByCode = existingReports.ToDictionary(r => r.Code, StringComparer.Ordinal);
        foreach (var (code, description, category, requiredFeatureKey, managerOnly, supportsPdf, sortOrder) in ReportCatalog)
        {
            if (reportsByCode.TryGetValue(code, out var report))
            {
                report.Description = description;
                report.Category = category;
                report.RequiredFeatureKey = requiredFeatureKey;
                report.ManagerOnly = managerOnly;
                report.SupportsPdf = supportsPdf;
                report.SortOrder = sortOrder;
            }
            else
            {
                context.Reports.Add(new Report(code, description, reportType, true, category, requiredFeatureKey, managerOnly, supportsPdf, sortOrder));
            }
        }
        await context.SaveChangesAsync();
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

        // Platform default notification templates are NOT seeded: localized
        // text never lives in the database. Defaults come from the NotificationDefaultMessages
        // resources at render time; the templates table holds account-authored overrides only.
        // Remove rows an earlier initializer version may have seeded.
        var seededDefaults = await context.NotificationTemplates.Where(t => t.AccountId == null).ToListAsync();
        if (seededDefaults.Count > 0)
        {
            context.NotificationTemplates.RemoveRange(seededDefaults);
            await context.SaveChangesAsync();
        }

        // One-time gating backfill: rule CRUD is now gated by the `notifications`
        // feature, so every account that already has NotificationRule rows gets an enabled feature
        // row. Idempotent; runs through the entity writer path, never raw SQL.
        var ruleAccounts = await context.NotificationRules.Select(r => r.AccountId).Distinct().ToListAsync();
        var gatedAccounts = await context.AccountFeatures
            .Where(f => f.FeatureKey == FeatureKeys.Notifications)
            .Select(f => f.AccountId)
            .Distinct()
            .ToListAsync();
        var missing = ruleAccounts.Except(gatedAccounts).ToList();
        if (missing.Count > 0)
        {
            foreach (var accountId in missing)
            {
                context.AccountFeatures.Add(new AccountFeature(accountId, FeatureKeys.Notifications, true, "standard", "migration", null, null, null));
            }
            await context.SaveChangesAsync();
        }
    }
}
