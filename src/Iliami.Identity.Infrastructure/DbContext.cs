using Iliami.Identity.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Iliami.Identity.Infrastructure;

public class DbContext(IConfiguration configuration, ILoggerFactory loggerFactory) : Microsoft.EntityFrameworkCore.DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<IdentityEvent> IdentityEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString =
            configuration.GetConnectionString("Postgres");

        optionsBuilder
            .UseNpgsql(connectionString)
            .UseLoggerFactory(loggerFactory)
            .EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DbContext).Assembly);
    }
}