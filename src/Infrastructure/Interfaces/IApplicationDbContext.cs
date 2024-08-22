﻿namespace TrackHub.Manager.Infrastructure.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; set; }
    DbSet<Credential> Credentials { get; set; }
    DbSet<Transporter> Transporters { get; set; }
    DbSet<TransporterGroup> TransportersGroup { get; set; }
    DbSet<Device> Devices { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Operator> Operators { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserGroup> UsersGroup { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
