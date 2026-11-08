using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using CargoModel = wt_lab8_8razor_postgresql_yushkevich.Models.Cargo;

namespace wt_lab8_8razor_postgresql_yushkevich.Pages.Cargo;

public class DetailsModel : PageModel
{
    private readonly AppDbContext _context;

    public DetailsModel(AppDbContext context)
    {
        _context = context;
    }

    public CargoModel? Cargo { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Cargo = await _context.Cargoes.FindAsync(id);

        if (Cargo is null)
        {
            return NotFound();
        }

        return Page();
    }
}
