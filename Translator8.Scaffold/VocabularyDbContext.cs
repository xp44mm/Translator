using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Translator8.Scaffold;

public partial class VocabularyDbContext : DbContext
{
    public VocabularyDbContext()
    {
    }

    public VocabularyDbContext(DbContextOptions<VocabularyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Word> Word { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=D:\\Application Data\\vocabulary.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>(entity =>
        {
            entity.Property(e => e.English).UseCollation("NOCASE");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
