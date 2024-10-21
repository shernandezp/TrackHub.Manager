using System.Reflection;
using EFCore.BulkExtensions;

namespace TrackHub.Manager.Infrastructure.ManagerDB;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

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

    public async Task BulkInsertAsync<T>(IList<T> entities, string? property = null, CancellationToken cancellationToken = default) where T : class
    {
        var bulkConfig = new BulkConfig();
        if (!string.IsNullOrEmpty(property)) 
        {
            bulkConfig.UpdateByProperties = [property];
        }
        await this.BulkInsertOrUpdateAsync(entities, bulkConfig, cancellationToken: cancellationToken);
    }
}
