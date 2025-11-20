using Microsoft.EntityFrameworkCore;

namespace trifork.Models;

public class FoosballContext : DbContext
{
    public FoosballContext(DbContextOptions<FoosballContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MatchModel>()
            .HasOne(m => m.Team1Player1)
            .WithMany()
            .HasForeignKey(m => m.Team1Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MatchModel>()
            .HasOne(m => m.Team1Player2)
            .WithMany()
            .HasForeignKey(m => m.Team1Player2Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MatchModel>()
            .HasOne(m => m.Team2Player1)
            .WithMany()
            .HasForeignKey(m => m.Team2Player1Id)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MatchModel>()
            .HasOne(m => m.Team2Player2)
            .WithMany()
            .HasForeignKey(m => m.Team2Player2Id)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public DbSet<PlayerModel> Players { get; set; }
    public DbSet<MatchModel> Matches { get; set; }
}