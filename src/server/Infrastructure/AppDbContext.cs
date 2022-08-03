using Microsoft.EntityFrameworkCore;
using Server.Api.Models.Database;

namespace Server.Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
    {
    }

    /// <summary>
    /// Create tables for dummy
    /// </summary>
    public DbSet<Db_Dummy> Db_Dummy { get; set; } = default!;

    /// <summary>
    /// Override model on creating
    /// </summary>
    /// <param name="modelBuilder"></param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        // Dummy
        _ = new OnModelCreatingDummy(modelBuilder);

        

    }



}
