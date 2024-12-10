using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace practice_hothouse.Models;

public partial class DbHothouseContext : DbContext
{
    public DbHothouseContext()
    {
    }

    public DbHothouseContext(DbContextOptions<DbHothouseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<HarvestHistory> HarvestHistories { get; set; }

    public virtual DbSet<Hectare> Hectares { get; set; }

    public virtual DbSet<Plot> Plots { get; set; }

    public virtual DbSet<Seed> Seeds { get; set; }

    public virtual DbSet<Sowing> Sowings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-NSNU3CD4\\SQLEXPRESS;Database=db_hothouse;Trusted_Connection=true;TrustServerCertificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<HarvestHistory>(entity =>
        {
            entity.HasKey(e => e.HarvestHistoryId).HasName("PK__HarvestH__58B7C1C9CDFD606A");

            entity.ToTable("HarvestHistory");

            entity.Property(e => e.HarvestHistoryId).HasColumnName("harvestHistoryID");
            entity.Property(e => e.HarvestDate).HasColumnName("harvestDate");
            entity.Property(e => e.HarvestedAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("harvestedAmount");
            entity.Property(e => e.IsArhive)
                .HasDefaultValue(0)
                .HasColumnName("isArhive");
            entity.Property(e => e.SowingId).HasColumnName("sowingID");
            entity.Property(e => e.UserId).HasColumnName("userID");

            entity.HasOne(d => d.Sowing).WithMany(p => p.HarvestHistories)
                .HasForeignKey(d => d.SowingId)
                .HasConstraintName("FK_HarvestHistory_Sowing");

            entity.HasOne(d => d.User).WithMany(p => p.HarvestHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_HarvestHistory_User");
        });

        modelBuilder.Entity<Hectare>(entity =>
        {
            entity.HasKey(e => e.HectareId).HasName("PK__Hectare__3A97A4D5AD405C5E");

            entity.ToTable("Hectare");

            entity.Property(e => e.HectareId).HasColumnName("hectareID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.HectareName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("hectareName");
            entity.Property(e => e.PlotId).HasColumnName("plotID");

            entity.HasOne(d => d.Plot).WithMany(p => p.Hectares)
                .HasForeignKey(d => d.PlotId)
                .HasConstraintName("FK_Hectare_Plot");
        });

        modelBuilder.Entity<Plot>(entity =>
        {
            entity.HasKey(e => e.PlotId).HasName("PK__Plot__5A47124B54C5DDA7");

            entity.ToTable("Plot");

            entity.Property(e => e.PlotId).HasColumnName("plotID");
            entity.Property(e => e.Cover)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("cover");
            entity.Property(e => e.PlotName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("plotName");
        });

        modelBuilder.Entity<Seed>(entity =>
        {
            entity.HasKey(e => e.SeedId).HasName("PK__Seed__F7D3D04978352CC7");

            entity.ToTable("Seed");

            entity.Property(e => e.SeedId).HasColumnName("seedID");
            entity.Property(e => e.AdditionalNotes)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("additionalNotes");
            entity.Property(e => e.DaysToFirstHarvest).HasColumnName("daysToFirstHarvest");
            entity.Property(e => e.DaysToGermination).HasColumnName("daysToGermination");
            entity.Property(e => e.HarvestFrequency).HasColumnName("harvestFrequency");
            entity.Property(e => e.NumberOfHarvests).HasColumnName("numberOfHarvests");
            entity.Property(e => e.SeedPlantingMethod)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("seedPlantingMethod");
            entity.Property(e => e.SeedType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("seedType");
            entity.Property(e => e.SeedVariety)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("seedVariety");
        });

        modelBuilder.Entity<Sowing>(entity =>
        {
            entity.HasKey(e => e.SowingId).HasName("PK__Sowing__EF6DE3F44A228D6B");

            entity.ToTable("Sowing");

            entity.Property(e => e.SowingId).HasColumnName("sowingID");
            entity.Property(e => e.ActualYield)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("actualYield");
            entity.Property(e => e.ExpectedYield)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("expectedYield");
            entity.Property(e => e.HectareId).HasColumnName("hectareID");
            entity.Property(e => e.IsArhive)
                .HasDefaultValue(0)
                .HasColumnName("isArhive");
            entity.Property(e => e.IsHarvestClosed).HasColumnName("isHarvestClosed");
            entity.Property(e => e.NumberInSeason).HasColumnName("numberInSeason");
            entity.Property(e => e.NumberOfPlantedSeeds).HasColumnName("numberOfPlantedSeeds");
            entity.Property(e => e.SeedId).HasColumnName("seedID");
            entity.Property(e => e.SowingDate).HasColumnName("sowingDate");

            entity.HasOne(d => d.Hectare).WithMany(p => p.Sowings)
                .HasForeignKey(d => d.HectareId)
                .HasConstraintName("FK_Sowing_Hectare");

            entity.HasOne(d => d.Seed).WithMany(p => p.Sowings)
                .HasForeignKey(d => d.SeedId)
                .HasConstraintName("FK_Sowing_Seed");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__CB9A1CDFBCF7C043");

            entity.ToTable("User");

            entity.HasIndex(e => e.Login, "UQ__User__7838F272864F52EA").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("userID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.IsArhive)
                .HasDefaultValue(0)
                .HasColumnName("isArhive");
            entity.Property(e => e.JobTitle)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("jobTitle");
            entity.Property(e => e.Login)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("login");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.PlotId).HasColumnName("plotID");
            entity.Property(e => e.UserFname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("userFName");
            entity.Property(e => e.UserLname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("userLName");
            entity.Property(e => e.UserSname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("userSName");

            entity.HasOne(d => d.Plot).WithMany(p => p.Users)
                .HasForeignKey(d => d.PlotId)
                .HasConstraintName("FK_User_Plot");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
