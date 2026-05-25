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

using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TrackHub.Manager.Infrastructure;
using TrackHub.Manager.Infrastructure.Interfaces;
using TrackHub.Manager.Infrastructure.Readers;
using TrackHub.Manager.Infrastructure.Writers;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        // Configure NpgsqlDataSourceBuilder and enable dynamic JSON serialization
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        dataSourceBuilder.ConfigureJsonOptions(new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        });
        dataSourceBuilder.EnableDynamicJson();
        var dataSource = dataSourceBuilder.Build();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(dataSource, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddHeaderPropagation(o => o.Headers.Add("Authorization"));

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<IAccountSettingsWriter, AccountSettingsWriter>();
        services.AddScoped<IAccountSettingsReader, AccountSettingsReader>();
        services.AddScoped<IAccountWriter, AccountWriter>();
        services.AddScoped<IAccountReader, AccountReader>();
        services.AddScoped<ICredentialWriter, CredentialWriter>();
        services.AddScoped<ICredentialReader, CredentialReader>();
        services.AddScoped<ITransporterWriter, TransporterWriter>();
        services.AddScoped<ITransporterPositionWriter, TransporterPositionWriter>();
        services.AddScoped<ITransporterGroupWriter, TransporterGroupWriter>();
        services.AddScoped<IDeviceWriter, DeviceWriter>();
        services.AddScoped<IDeviceReader, DeviceReader>();
        services.AddScoped<IDeviceTransporterReader, DeviceTransporterReader>();
        services.AddScoped<ITransporterReader, TransporterReader>();
        services.AddScoped<ITransporterPositionReader, TransporterPositionReader>();
        services.AddScoped<ITransporterTypeWriter, TransporterTypeWriter>();
        services.AddScoped<ITransporterTypeReader, TransporterTypeReader>();
        services.AddScoped<IGroupWriter, GroupWriter>();
        services.AddScoped<IGroupReader, GroupReader>();
        services.AddScoped<IOperatorWriter, OperatorWriter>();
        services.AddScoped<IOperatorReader, OperatorReader>();
        services.AddScoped<IReportWriter, ReportWriter>();
        services.AddScoped<IReportReader, ReportReader>();
        services.AddScoped<IDriverReader, DriverReader>();
        services.AddScoped<IDriverWriter, DriverWriter>();
        services.AddScoped<IGroupVisibilityReader, GroupVisibilityReader>();
        services.AddScoped<IAccountFeatureReader, AccountFeatureReader>();
        services.AddScoped<IAccountFeatureWriter, AccountFeatureWriter>();
        services.AddScoped<IAuditEventReader, AuditEventReader>();
        services.AddScoped<IAuditEventWriter, AuditEventWriter>();
        services.AddScoped<IDocumentReader, DocumentReader>();
        services.AddScoped<IDocumentWriter, DocumentWriter>();
        services.AddScoped<INotificationReader, NotificationReader>();
        services.AddScoped<INotificationWriter, NotificationWriter>();
        services.AddScoped<IAlertEventReader, AlertEventReader>();
        services.AddScoped<IAlertEventWriter, AlertEventWriter>();
        services.AddScoped<IBackgroundJobRunReader, BackgroundJobRunReader>();
        services.AddScoped<IBackgroundJobRunWriter, BackgroundJobRunWriter>();
        services.AddScoped<IPublicLinkGrantReader, PublicLinkGrantReader>();
        services.AddScoped<IPublicLinkGrantWriter, PublicLinkGrantWriter>();
        services.AddScoped<IAccountSupportGrantReader, AccountSupportGrantReader>();
        services.AddScoped<IAccountSupportGrantWriter, AccountSupportGrantWriter>();
        services.AddScoped<IUserWriter, UserWriter>();
        services.AddScoped<IUserReader, UserReader>();
        services.AddScoped<IUserSettingsWriter, UserSettingsWriter>();
        services.AddScoped<IUserSettingsReader, UserSettingsReader>();
        services.AddScoped<IUserGroupWriter, UserGroupWriter>();
        services.AddScoped<ITransporterDeviceAssignmentReader, TransporterDeviceAssignmentReader>();
        services.AddScoped<ITransporterDeviceAssignmentWriter, TransporterDeviceAssignmentWriter>();
        services.AddScoped<IOperatorHealthCheckReader, OperatorHealthCheckReader>();
        services.AddScoped<IOperatorHealthCheckWriter, OperatorHealthCheckWriter>();
        services.AddScoped<IOperatorSyncRunReader, OperatorSyncRunReader>();
        services.AddScoped<IOperatorSyncRunWriter, OperatorSyncRunWriter>();
        services.AddScoped<ITransporterPositionHistoryReader, TransporterPositionHistoryReader>();
        services.AddScoped<ITransporterPositionHistoryWriter, TransporterPositionHistoryWriter>();
        services.AddScoped<IGpsIntegrationDashboardReader, GpsIntegrationDashboardReader>();
        services.AddScoped<IPositionRetentionPolicyReader, PositionRetentionPolicyReader>();
        services.AddScoped<IPositionRetentionPolicyWriter, PositionRetentionPolicyWriter>();
        services.AddMemoryCache();
        services.AddScoped<Common.Application.Interfaces.IFeatureFlagService, TrackHub.Manager.Infrastructure.ManagerDB.Services.FeatureFlagService>();

        return services;
    }
}
