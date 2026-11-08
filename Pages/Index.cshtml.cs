using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using CargoModel = wt_lab8_8razor_postgresql_yushkevich.Models.Cargo;

namespace wt_lab8_8razor_postgresql_yushkevich.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IReadOnlyList<CargoModel> Cargoes { get; private set; } = Array.Empty<CargoModel>();
    public IReadOnlyList<CargoModel> RecentCargoes { get; private set; } = Array.Empty<CargoModel>();

    public int PositionsCount { get; private set; }
    public int AvailableCount { get; private set; }
    public int TotalQuantity { get; private set; }
    public decimal TotalWeight { get; private set; }

    public async Task OnGetAsync()
    {
        Cargoes = await _context.Cargoes.ToListAsync();
        RecentCargoes = await _context.Cargoes
            .OrderByDescending(cargo => cargo.Id)
            .Take(3)
            .ToListAsync();

        PositionsCount = Cargoes.Count;
        AvailableCount = Cargoes.Count(cargo => cargo.IsAvailable);
        TotalQuantity = Cargoes.Sum(cargo => cargo.Quantity);
        TotalWeight = Cargoes.Sum(cargo => cargo.TotalWeight);
    }
}
