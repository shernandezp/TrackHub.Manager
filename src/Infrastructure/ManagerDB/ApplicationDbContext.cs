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

using System.Reflection;

namespace TrackHub.Manager.Infrastructure.ManagerDB;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountSettings> AccountSettings { get; set; }
    public DbSet<Credential> Credentials { get; set; }
    public DbSet<Transporter> Transporters { get; set; }
    public DbSet<TransporterGroup> TransportersGroup { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<TransporterPosition> TransporterPositions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UsersGroup { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

}
