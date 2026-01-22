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
        services.AddScoped<IUserWriter, UserWriter>();
        services.AddScoped<IUserReader, UserReader>();
        services.AddScoped<IUserSettingsWriter, UserSettingsWriter>();
        services.AddScoped<IUserSettingsReader, UserSettingsReader>();
        services.AddScoped<IUserGroupWriter, UserGroupWriter>();

        return services;
    }
}
