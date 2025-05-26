using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using demExamSix.Models;

namespace demExamSix.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<MaterialType> MaterialTypes { get; set; }

    public virtual DbSet<UnitMeasurement> UnitMeasurements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=DemExamSix;Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.MaterialId).HasName("PK_Material_import");

            entity.ToTable("Material");

            entity.Property(e => e.MaterialId)
                .ValueGeneratedNever()
                .HasColumnName("materialId");
            entity.Property(e => e.CountInPackage).HasColumnName("countInPackage");
            entity.Property(e => e.CountInStock).HasColumnName("countInStock");
            entity.Property(e => e.MeasurementId).HasColumnName("measurementId");
            entity.Property(e => e.MinCount).HasColumnName("minCount");
            entity.Property(e => e.PricePerUnitOfMaterial)
                .HasColumnType("numeric(18, 2)")
                .HasColumnName("pricePerUnitOfMaterial");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
            entity.Property(e => e.TypeMaterialId).HasColumnName("typeMaterialId");

            entity.HasOne(d => d.Measurement).WithMany(p => p.Materials)
                .HasForeignKey(d => d.MeasurementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_Unit_measurement");

            entity.HasOne(d => d.TypeMaterial).WithMany(p => p.Materials)
                .HasForeignKey(d => d.TypeMaterialId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Material_Material_type");
        });

        modelBuilder.Entity<MaterialType>(entity =>
        {
            entity.HasKey(e => e.TypeMaterialId);

            entity.ToTable("Material_type");

            entity.Property(e => e.TypeMaterialId)
                .ValueGeneratedNever()
                .HasColumnName("typeMaterialId");
            entity.Property(e => e.TitleMaterialType)
                .HasMaxLength(100)
                .HasColumnName("titleMaterialType");
        });

        modelBuilder.Entity<UnitMeasurement>(entity =>
        {
            entity.HasKey(e => e.MeasurementId).HasName("PK_Unit_measurement_import");

            entity.ToTable("Unit_measurement");

            entity.Property(e => e.MeasurementId)
                .ValueGeneratedNever()
                .HasColumnName("measurementId");
            entity.Property(e => e.TitleMeasurement)
                .HasMaxLength(100)
                .HasColumnName("titleMeasurement");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
