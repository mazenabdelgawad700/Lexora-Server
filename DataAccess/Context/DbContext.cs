using Lexora.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lexora.DataAccess.Context
{
  public class LexoraDbContext : DbContext
  {
    public LexoraDbContext(DbContextOptions<LexoraDbContext> options)
        : base(options)
    {
    }

    public DbSet<VocabularyEntry> VocabularyEntry { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<VocabularyEntry>()
          .ToTable(nameof(VocabularyEntry));

      var entity = modelBuilder.Entity<VocabularyEntry>();

      // ----------------------------
      // Indexes (NON-CLUSTERED by default in EF Core unless specified)
      // ----------------------------

      entity.HasIndex(x => x.Word)
            .HasDatabaseName("IX_VocabularyEntry_Word");

      entity.HasIndex(x => x.Definition)
            .HasDatabaseName("IX_VocabularyEntry_Definition");

      // ----------------------------
      // No uniqueness constraints (explicit clarity)
      // ----------------------------

      entity.HasIndex(x => x.Word).IsUnique(false);
      entity.HasIndex(x => x.Definition).IsUnique(false);

      // ----------------------------
      // Soft delete filter (important)
      // ----------------------------
      entity.HasQueryFilter(x => !x.IsDeleted);

      // ----------------------------
      // Column constraints (extra safety)
      // ----------------------------
      entity.Property(x => x.Word)
            .HasMaxLength(200)
            .IsRequired();

      entity.Property(x => x.Definition)
            .HasMaxLength(2000)
            .IsRequired();

      entity.Property(x => x.Example)
            .HasMaxLength(2000)
            .IsRequired();
    }

    // ----------------------------
    // Optional: global soft delete handling (recommended pattern)
    // ----------------------------
    public override int SaveChanges()
    {
      HandleSoftDelete();
      HandleUpdateEntity();
      return base.SaveChanges();
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
      HandleSoftDelete();
      HandleUpdateEntity();
      return base.SaveChangesAsync(cancellationToken);
    }
    private void HandleSoftDelete()
    {
      foreach (var entry in ChangeTracker.Entries<VocabularyEntry>())
      {
        if (entry.State == EntityState.Deleted)
        {
          entry.State = EntityState.Modified;
          entry.Entity.IsDeleted = true;
          entry.Entity.UpdatedAt = DateTime.UtcNow;
        }

        if (entry.State == EntityState.Modified)
        {
          entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
      }
    }
    private void HandleUpdateEntity()
    {
      foreach (var entry in ChangeTracker.Entries<VocabularyEntry>())
      {
        if (entry.State == EntityState.Modified)
          entry.Entity.UpdatedAt = DateTime.UtcNow;
      }
    }
  }
}