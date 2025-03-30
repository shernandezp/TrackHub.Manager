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

using TrackHub.Manager.Infrastructure.Entities;

namespace TrackHub.Manager.Infrastructure.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; set; }
    DbSet<AccountSettings> AccountSettings { get; set; }
    DbSet<Credential> Credentials { get; set; }
    DbSet<Transporter> Transporters { get; set; }
    DbSet<TransporterGroup> TransportersGroup { get; set; }
    DbSet<Device> Devices { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Operator> Operators { get; set; }
    DbSet<Report> Reports { get; set; }
    DbSet<TransporterPosition> TransporterPositions { get; set; }
    DbSet<TransporterType> TransporterTypes { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserGroup> UsersGroup { get; set; }
    DbSet<UserSettings> UserSettings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
