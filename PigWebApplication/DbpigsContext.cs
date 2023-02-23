using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PigWebApplication;

public partial class DbpigsContext : DbContext
{
    public DbpigsContext()
    {
    }

    public DbpigsContext(DbContextOptions<DbpigsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Breed> Breeds { get; set; }

    public virtual DbSet<FeedMixture> FeedMixtures { get; set; }

    public virtual DbSet<FeedType> FeedTypes { get; set; }

    public virtual DbSet<Injection> Injections { get; set; }

    public virtual DbSet<Medicine> Medicines { get; set; }

    public virtual DbSet<Pig> Pigs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP\\SQLEXPRESS; Database=DBPigs; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Breed>(entity =>
        {
            entity.ToTable("Breed");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Direction)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("direction");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<FeedMixture>(entity =>
        {
            entity.ToTable("FeedMixture");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Barley).HasColumnName("barley");
            entity.Property(e => e.Corn).HasColumnName("corn");
            entity.Property(e => e.Name)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Oilcake).HasColumnName("oilcake");
            entity.Property(e => e.Pea).HasColumnName("pea");
            entity.Property(e => e.Wheet).HasColumnName("wheet");
        });

        modelBuilder.Entity<FeedType>(entity =>
        {
            entity.ToTable("FeedType");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgeFinish).HasColumnName("age_finish");
            entity.Property(e => e.AgeStart).HasColumnName("age_start");
            entity.Property(e => e.BreedId).HasColumnName("breed_id");
            entity.Property(e => e.FeedmixId).HasColumnName("feedmix_id");
            entity.Property(e => e.QuantityPerBig).HasColumnName("quantity_per_big");

            entity.HasOne(d => d.Breed).WithMany(p => p.FeedTypes)
                .HasForeignKey(d => d.BreedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeedType_Breed");

            entity.HasOne(d => d.Feedmix).WithMany(p => p.FeedTypes)
                .HasForeignKey(d => d.FeedmixId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_FeedType_FeedMixture");
        });

        modelBuilder.Entity<Injection>(entity =>
        {
            entity.ToTable("Injection");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("date");
            entity.Property(e => e.MedicineId).HasColumnName("medicine_id");
            entity.Property(e => e.Note)
                .HasMaxLength(100)
                .HasColumnName("note");
            entity.Property(e => e.PigId).HasColumnName("pig_id");

            entity.HasOne(d => d.Medicine).WithMany(p => p.Injections)
                .HasForeignKey(d => d.MedicineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Injection_Medicine");

            entity.HasOne(d => d.Pig).WithMany(p => p.Injections)
                .HasForeignKey(d => d.PigId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Injection_Pig");
        });

        modelBuilder.Entity<Medicine>(entity =>
        {
            entity.ToTable("Medicine");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Pig>(entity =>
        {
            entity.ToTable("Pig");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BirthDate)
                .HasColumnType("date")
                .HasColumnName("birth_date");
            entity.Property(e => e.BreedId).HasColumnName("breed_id");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Note)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("note");

            entity.HasOne(d => d.Breed).WithMany(p => p.Pigs)
                .HasForeignKey(d => d.BreedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Pig_Breed");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
