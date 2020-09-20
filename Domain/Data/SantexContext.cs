using Domain.Data.Configurations;
using Domain.Models;
using Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Domain.Data
{
    public partial class SantexContext : DbContext, IUnitOfWork
    {
        public SantexContext()
        {
        }

        public SantexContext(DbContextOptions<SantexContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Competition> Competitions { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Player> Players { get; set; }
        public virtual DbSet<CompetitionTeam> CompetitionTeams { get; set; }
        public virtual DbSet<TeamPlayer> TeamPlayers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CompetitionTeamEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TeamPlayerEntityTypeConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
