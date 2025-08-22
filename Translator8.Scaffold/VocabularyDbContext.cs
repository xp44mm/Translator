using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Translator8.Scaffold;

public partial class VocabularyDbContext : DbContext
{
    public virtual DbSet<Word> Word { get; set; }

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
