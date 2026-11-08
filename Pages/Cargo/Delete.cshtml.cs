using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using CargoModel = wt_lab8_8razor_postgresql_yushkevich.Models.Cargo;

namespace wt_lab8_8razor_postgresql_yushkevich.Pages.Cargo;

public class DeleteModel : PageModel
{
    private readonly AppDbContext _context;

    public DeleteModel(AppDbContext context)
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var cargo = await _context.Cargoes.FindAsync(id);

        if (cargo is null)
        {
            return NotFound();
        }

        _context.Cargoes.Remove(cargo);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Груз «{cargo.Name}» удалён из каталога.";

        return RedirectToPage("./Index");
    }
}
