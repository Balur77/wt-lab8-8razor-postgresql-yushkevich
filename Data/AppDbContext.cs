using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Models;

namespace wt_lab8_8razor_postgresql_yushkevich.Data;

/// <summary>
/// Контекст базы данных PostgreSQL для учётной системы грузов CargoFlow.
/// Обеспечивает доступ к таблице Cargoes через Entity Framework Core.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Cargo> Cargoes => Set<Cargo>();
}
