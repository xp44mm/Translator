using System.Configuration;

using Microsoft.EntityFrameworkCore;

namespace Translator.ef
{
    public partial class TranslateContext : DbContext
    {
        public TranslateContext()
        {
        }

        public TranslateContext(
            DbContextOptions<TranslateContext> opts
            ) : base(opts)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString =
                    ConfigurationManager.ConnectionStrings["translate"].ConnectionString;

                optionsBuilder.UseSqlServer(connectionString);
                base.OnConfiguring(optionsBuilder);
            }
        }

        public virtual DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Word>(entity =>
            {
                entity.Property(e => e.English).ValueGeneratedNever();
            });
        }
    }
}
