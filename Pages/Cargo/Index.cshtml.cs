using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using CargoModel = wt_lab8_8razor_postgresql_yushkevich.Models.Cargo;

namespace wt_lab8_8razor_postgresql_yushkevich.Pages.Cargo;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public string? Search { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Category { get; set; }

    public IReadOnlyList<CargoModel> Cargoes { get; private set; } = Array.Empty<CargoModel>();
    public IEnumerable<SelectListItem> Categories { get; private set; } = Array.Empty<SelectListItem>();

    public int TotalCount { get; private set; }
    public bool HasFilters => !string.IsNullOrWhiteSpace(Search) || !string.IsNullOrWhiteSpace(Category);

    public async Task OnGetAsync()
    {
        var query = _context.Cargoes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(Search))
        {
            var term = Search.Trim();
            query = query.Where(cargo =>
                cargo.Name.Contains(term) ||
                cargo.Location.Contains(term) ||
                cargo.Description.Contains(term));
        }

        if (!string.IsNullOrWhiteSpace(Category))
        {
            query = query.Where(cargo => cargo.Category == Category);
        }

        TotalCount = await _context.Cargoes.CountAsync();
        Cargoes = await query.OrderBy(cargo => cargo.Id).ToListAsync();

        Categories = await _context.Cargoes
            .Select(cargo => cargo.Category)
            .Distinct()
            .OrderBy(category => category)
            .Select(category => new SelectListItem(category, category))
            .ToListAsync();
    }
}
