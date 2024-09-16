namespace TrackHub.Manager.Infrastructure.ManagerDB.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Account> Accounts { get; set; }
    public DbSet<AccountSettings> AccountSettings { get; set; }
    DbSet<Credential> Credentials { get; set; }
    DbSet<Transporter> Transporters { get; set; }
    DbSet<TransporterGroup> TransportersGroup { get; set; }
    DbSet<Device> Devices { get; set; }
    DbSet<Group> Groups { get; set; }
    DbSet<Operator> Operators { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserGroup> UsersGroup { get; set; }
    public DbSet<UserSettings> UserSettings { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
