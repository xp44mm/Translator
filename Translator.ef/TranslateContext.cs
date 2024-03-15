using Microsoft.EntityFrameworkCore;

using System;
using System.IO;

namespace Translator.ef
{
    public partial class TranslateContext : DbContext
    {
        public string DbPath { get; }

        public TranslateContext()
        {
            var filename = "vocabulary.db";
            var current = Path.Combine(Environment.CurrentDirectory, filename);

            //C:\Users\cuishengli\AppData\Local
            //var folder = Environment.SpecialFolder.LocalApplicationData;
            //var path = Environment.GetFolderPath(folder);

            //如果当前目录存在数据库用当前目录，否则从
            this.DbPath =
                File.Exists(current) ? current :
                Path.Combine(@"D:\Application Data", filename);
        }

        //public TranslateContext(
        //    DbContextOptions<TranslateContext> opts
        //    ) : base(opts)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (!optionsBuilder.IsConfigured)
            //{
            //    var connectionString =
            //        ConfigurationManager.ConnectionStrings["translate"].ConnectionString;

            //    optionsBuilder.UseSqlServer(connectionString);
            //    base.OnConfiguring(optionsBuilder);
            //}
            optionsBuilder.UseSqlite($"Data Source={this.DbPath}");

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
