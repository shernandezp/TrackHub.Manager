﻿using System.Reflection;

namespace TrackHub.Manager.Infrastructure;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Credential> Credentials { get; set; }
    public DbSet<Device> Devices { get; set; }
    public DbSet<DeviceGroup> DeviceGroups { get; set; }
    public DbSet<DeviceOperator> DeviceOperators { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Operator> Operators { get; set; }
    public DbSet<Transporter> Transporters { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserGroup> UserGroups { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }
}
