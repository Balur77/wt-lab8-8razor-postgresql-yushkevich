using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using CargoModel = wt_lab8_8razor_postgresql_yushkevich.Models.Cargo;

namespace wt_lab8_8razor_postgresql_yushkevich.Pages.Cargo;

public class CreateModel : PageModel
{
    private readonly AppDbContext _context;

    public CreateModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty]
    public CargoModel Cargo { get; set; } = new();

    public IEnumerable<string> Categories { get; private set; } = Array.Empty<string>();

    public async Task OnGetAsync()
    {
        Categories = await LoadCategoriesAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Categories = await LoadCategoriesAsync();
            return Page();
        }

        Cargo.Name = Cargo.Name.Trim();
        Cargo.Category = Cargo.Category.Trim();
        Cargo.Location = Cargo.Location.Trim();
        Cargo.Description = Cargo.Description.Trim();

        _context.Cargoes.Add(Cargo);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            $"Груз «{Cargo.Name}» добавлен в каталог (ID: {Cargo.Id}).";

        return RedirectToPage("./Index");
    }

    private async Task<IEnumerable<string>> LoadCategoriesAsync() =>
        await _context.Cargoes
            .Select(cargo => cargo.Category)
            .Distinct()
            .OrderBy(category => category)
            .ToListAsync();
}
