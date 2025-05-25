using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Yongqing.Database;

namespace Yongqing.Databases.Test;
public partial class YongqingContext : DbContext
{
    private readonly IConfiguration _configuration;

    public YongqingContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public DbSet<Books> Books { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("Yongqing") ?? string.Empty, options => options.EnableRetryOnFailure());
        base.OnConfiguring(optionsBuilder);
    }

    public async Task<int> ExecuteSqlAsync(string sql, params SqlParameter[] parameters)
        => await this.Database.ExecuteSqlRawAsync(sql, parameters);

    public async Task<IReadOnlyList<TSheet>> GetResultAsync<TSheet>(string sql, params SqlParameter[] parameters) where TSheet : class
        => await this.Set<TSheet>().FromSqlRaw(sql, parameters).AsNoTracking().ToListAsync();
}
