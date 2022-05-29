using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace LibraryWebApplication1
{
    public partial class DBLibraryContext : DbContext
    {
        public DBLibraryContext()
        {
        }

        public DBLibraryContext(DbContextOptions<DBLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Game> Games { get; set; } = null!;
        public virtual DbSet<Stadium> Stadia { get; set; } = null!;
        public virtual DbSet<Team> Teams { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server= DESKTOP-4IDOTFT; Database=DBLibrary; Trusted_Connection=True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.HasKey(e => e.CiId);

                entity.ToTable("City");

                entity.Property(e => e.CiId)
                    .HasMaxLength(50)
                    .HasColumnName("CI_ID");

                entity.Property(e => e.CiCountry)
                    .HasMaxLength(50)
                    .HasColumnName("CI_Country");

                entity.Property(e => e.CiName)
                    .HasMaxLength(50)
                    .HasColumnName("CI_Name");

                entity.Property(e => e.CiPopulation).HasColumnName("CI_Population");

                entity.HasOne(d => d.CiCountryNavigation)
                    .WithMany(p => p.Cities)
                    .HasForeignKey(d => d.CiCountry)
                    .HasConstraintName("FK_City_Country1");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.CoId);

                entity.ToTable("Country");

                entity.Property(e => e.CoId)
                    .HasMaxLength(50)
                    .HasColumnName("CO_ID");

                entity.Property(e => e.CoName)
                    .HasMaxLength(50)
                    .HasColumnName("CO_Name");

                entity.Property(e => e.CoPopulation).HasColumnName("CO_Population");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.HasKey(e => e.GId);

                entity.ToTable("Game");

                entity.Property(e => e.GId)
                    .ValueGeneratedNever()
                    .HasColumnName("G_ID");

                entity.Property(e => e.GAttendance).HasColumnName("G_Attendance");

                entity.Property(e => e.GStadium)
                    .HasMaxLength(50)
                    .HasColumnName("G_Stadium");

                entity.Property(e => e.GTeamA)
                    .HasMaxLength(50)
                    .HasColumnName("G_TeamA");

                entity.Property(e => e.GTeamH)
                    .HasMaxLength(50)
                    .HasColumnName("G_TeamH");

                entity.HasOne(d => d.GStadiumNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.GStadium)
                    .HasConstraintName("FK_Game_Stadium");

                entity.HasOne(d => d.GTeamANavigation)
                    .WithMany(p => p.GameGTeamANavigations)
                    .HasForeignKey(d => d.GTeamA)
                    .HasConstraintName("FK_Game_Team1");

                entity.HasOne(d => d.GTeamHNavigation)
                    .WithMany(p => p.GameGTeamHNavigations)
                    .HasForeignKey(d => d.GTeamH)
                    .HasConstraintName("FK_Game_Team");
            });

            modelBuilder.Entity<Stadium>(entity =>
            {
                entity.HasKey(e => e.StId);

                entity.ToTable("Stadium");

                entity.Property(e => e.StId)
                    .HasMaxLength(50)
                    .HasColumnName("ST_ID");

                entity.Property(e => e.StCapacity).HasColumnName("ST_Capacity");

                entity.Property(e => e.StCity)
                    .HasMaxLength(50)
                    .HasColumnName("ST_City");

                entity.Property(e => e.StName)
                    .HasMaxLength(50)
                    .HasColumnName("ST_Name");

                entity.HasOne(d => d.StCityNavigation)
                    .WithMany(p => p.Stadia)
                    .HasForeignKey(d => d.StCity)
                    .HasConstraintName("FK_Stadium_City");
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TId);

                entity.ToTable("Team");

                entity.Property(e => e.TId)
                    .HasMaxLength(50)
                    .HasColumnName("T_ID");

                entity.Property(e => e.TCity)
                    .HasMaxLength(50)
                    .HasColumnName("T_City");

                entity.Property(e => e.TManager)
                    .HasMaxLength(50)
                    .HasColumnName("T_Manager");

                entity.Property(e => e.TName)
                    .HasMaxLength(50)
                    .HasColumnName("T_Name");

                entity.Property(e => e.TStadium)
                    .HasMaxLength(50)
                    .HasColumnName("T_Stadium");

                entity.HasOne(d => d.TCityNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.TCity)
                    .HasConstraintName("FK_Team_City");

                entity.HasOne(d => d.TStadiumNavigation)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.TStadium)
                    .HasConstraintName("FK_Team_Stadium");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
