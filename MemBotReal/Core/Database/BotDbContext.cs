using BotBase.Database;
using MemBotReal.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace MemBotReal.Database;

public abstract class BotDbContext : BotDbContextBase
{
    public DbSet<GuildConfig> GuildConfigs { get; set; }
    public DbSet<Case> Cases { get; set; }
    public DbSet<DelayedUnmute> DelayedUnmutes { get; set; }
    public DbSet<CustomCommand> CustomCommands { get; set; }

    protected BotDbContext(string connectionString) : base(connectionString)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DelayedUnmute>()
            .HasKey(x => new { x.GuildId, x.OffenderId });

        modelBuilder.Entity<Case>()
            .HasIndex(x => new { x.GuildId, x.OffenderId });

        modelBuilder.Entity<CustomCommand>()
            .HasIndex(x => new { x.GuildId, x.Name});
    }
}