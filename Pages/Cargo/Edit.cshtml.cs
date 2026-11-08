using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using CargoModel = wt_lab8_8razor_postgresql_yushkevich.Models.Cargo;

namespace wt_lab8_8razor_postgresql_yushkevich.Pages.Cargo;

public class EditModel : PageModel
{
    private readonly AppDbContext _context;

    public EditModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CargoModel Cargo { get; set; } = new();

    public IEnumerable<string> Categories { get; private set; } = Array.Empty<string>();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var cargo = await _context.Cargoes.FindAsync(id);

        if (cargo is null)
        {
            return NotFound();
        }

        Cargo = cargo;
        Categories = await LoadCategoriesAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Categories = await LoadCategoriesAsync();
            return Page();
        }

        var existing = await _context.Cargoes.FindAsync(Cargo.Id);

        if (existing is null)
        {
            return NotFound();
        }

        existing.Name = Cargo.Name.Trim();
        existing.Category = Cargo.Category.Trim();
        existing.Quantity = Cargo.Quantity;
        existing.Location = Cargo.Location.Trim();
        existing.Weight = Cargo.Weight;
        existing.Description = Cargo.Description.Trim();
        existing.IsAvailable = Cargo.IsAvailable;

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Груз «{existing.Name}» успешно обновлён.";

        return RedirectToPage("./Index");
    }

    private async Task<IEnumerable<string>> LoadCategoriesAsync() =>
        await _context.Cargoes
            .Select(cargo => cargo.Category)
            .Distinct()
            .OrderBy(category => category)
            .ToListAsync();
}
