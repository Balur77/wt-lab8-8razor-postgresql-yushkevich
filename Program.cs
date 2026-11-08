using System.Globalization;
using Microsoft.EntityFrameworkCore;
using wt_lab8_8razor_postgresql_yushkevich.Data;
using wt_lab8_8razor_postgresql_yushkevich.Models;

// Десятичным разделителем в HTML-формах является точка, поэтому
// для модели связывания используем русскую культуру с точкой.
var appCulture = (CultureInfo)CultureInfo.GetCultureInfo("ru-RU").Clone();
appCulture.NumberFormat.NumberDecimalSeparator = ".";
appCulture.NumberFormat.CurrencyDecimalSeparator = ".";
CultureInfo.DefaultThreadCurrentCulture = appCulture;
CultureInfo.DefaultThreadCurrentUICulture = appCulture;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Строка подключения к PostgreSQL (lokally — appsettings.Development.json,
// на Render — переменная окружения ConnectionStrings__DefaultConnection).
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Применяем pending миграции при запуске (нужно для Render, где нет
// отдельного шага dotnet ef database update).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();

    // Стартовый набор грузов, если база пустая (аналог WT-5 CargoStore).
    if (!db.Cargoes.Any())
    {
        db.Cargoes.AddRange(SeedData());
        await db.SaveChangesAsync();
    }
}

// Render передаёт внешний порт через переменную окружения PORT.
// Приложение должно слушать все сетевые интерфейсы внутри контейнера.
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

app.Run($"http://0.0.0.0:{port}");

static List<Cargo> SeedData() =>
[
    new Cargo
    {
        Name = "Ноутбуки Dell Latitude 5440",
        Category = "Электроника",
        Quantity = 24,
        Location = "Сектор A-1, стеллаж 3",
        Weight = 2.4m,
        Description = "Партия офисных ноутбуков для оснащения новых рабочих мест. Гарантия производителя — 24 месяца.",
        IsAvailable = true
    },
    new Cargo
    {
        Name = "Офисные кресла ErgoPro",
        Category = "Мебель",
        Quantity = 18,
        Location = "Сектор B-2, зона сборки",
        Weight = 12.5m,
        Description = "Эргономичные кресла с регулируемой поддержкой поясницы. Поставка в разобранном виде.",
        IsAvailable = true
    },
    new Cargo
    {
        Name = "Кофемашины Jura E8",
        Category = "Бытовая техника",
        Quantity = 6,
        Location = "Сектор C-1, стеллаж 1",
        Weight = 9.8m,
        Description = "Профессиональные кофемашины для переговорных комнат и клиентских зон.",
        IsAvailable = true
    },
    new Cargo
    {
        Name = "Наборы строительного инструмента",
        Category = "Инструменты",
        Quantity = 45,
        Location = "Сектор D-3, паллета 12",
        Weight = 25m,
        Description = "Универсальные наборы инструментов для ремонтных бригад: дрели, шуруповёрты и расходники.",
        IsAvailable = true
    },
    new Cargo
    {
        Name = "Медицинские маски",
        Category = "Медицина",
        Quantity = 1200,
        Location = "Сектор E-2, стеллаж 5",
        Weight = 0.005m,
        Description = "Одноразовые трёхслойные маски в индивидуальной упаковке. Поставка коробами по 50 штук.",
        IsAvailable = true
    },
    new Cargo
    {
        Name = "Серверное оборудование Supermicro",
        Category = "Электроника",
        Quantity = 4,
        Location = "Сектор A-3, климат-зона",
        Weight = 18.6m,
        Description = "Стоечные серверы для модернизации дата-центра. Ожидают таможенного оформления.",
        IsAvailable = false
    }
];
