using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Translator8.Scaffold;

/// <summary>
/// 用于DI服务，无DI自动生成代码时不要添加OnConfiguring
/// </summary>
//void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

public partial class VocabularyDbContext : DbContext
{
    public VocabularyDbContext(string connectionString)
        : base(CreateOptions(connectionString))
    {
    }

    private static DbContextOptions<VocabularyDbContext> CreateOptions(string connectionString)
    {
        var builder = new DbContextOptionsBuilder<VocabularyDbContext>();
        builder.UseSqlite(connectionString);
        return builder.Options;
    }
}
