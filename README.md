# WT-8 Razor Pages + PostgreSQL (Entity Framework Core)

Учебное веб-приложение **CargoFlow** для учёта складских грузов, выполненное на
ASP.NET Core Razor Pages в рамках лабораторной работы WT-8. Данные хранятся в
PostgreSQL через Entity Framework Core (Code First + миграции).

## Репозиторий

https://github.com/Balur77/wt-lab8-8razor-postgresql-yushkevich

## Предметная область

Учёт грузов на складе (Cargo). Для каждой позиции хранятся: идентификатор,
название, категория, количество, место хранения, вес одной единицы, описание и
признак доступности. Общий вес партии и текстовый статус вычисляются в модели.

## Функциональность

- главная страница с показателями склада и последними добавленными грузами;
- каталог грузов в адаптивных карточках Bootstrap;
- поиск по названию, месту хранения и описанию;
- фильтрация по категории;
- страница подробной информации о грузе;
- добавление груза через форму с серверной и клиентской валидацией;
- редактирование груза;
- удаление груза с подтверждением;
- полный CRUD с хранением данных в PostgreSQL;
- публикация в Docker-контейнере на Render с удалённой базой данных PostgreSQL.

## Технологии

- C# 13 / .NET 9;
- ASP.NET Core Razor Pages;
- Entity Framework Core + Npgsql (PostgreSQL);
- Code First и миграции EF Core;
- Bootstrap 5 (локальные статические файлы);
- jQuery Validation / Unobtrusive Validation;
- Docker;
- Render Web Service.

## Структура проекта

```text
wt-lab8-8razor-postgresql-yushkevich/
├── Data/
│   └── AppDbContext.cs     # контекст EF Core, DbSet<Cargo>
├── Migrations/             # миграции EF Core (InitialCreate и последующие)
├── Models/
│   └── Cargo.cs            # модель предметной области
├── Pages/
│   ├── Cargo/
│   │   ├── Index.cshtml(.cs)    # каталог, поиск, фильтр
│   │   ├── Create.cshtml(.cs)   # добавление + валидация
│   │   ├── Details.cshtml(.cs)  # подробная карточка
│   │   ├── Edit.cshtml(.cs)     # редактирование
│   │   └── Delete.cshtml(.cs)   # удаление
│   ├── Shared/
│   │   ├── _Layout.cshtml       # общий layout и Bootstrap Navbar
│   │   ├── _CargoCard.cshtml    # переиспользуемая карточка
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Index.cshtml             # главная страница
│   ├── Index.cshtml.cs          # статистика для главной
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
├── Dockerfile
├── appsettings.json
├── appsettings.Development.json  # строка подключения PostgreSQL (без пароля в репозитории)
├── Program.cs                    # регистрация AppDbContext, PORT, миграции при старте
├── README.md
└── wt-lab8-8razor-postgresql-yushkevich.csproj
```

## Архитектура

```
Razor Pages
    ↓
PageModel
    ↓
AppDbContext
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

## Настройка строки подключения

Локальная разработка (`appsettings.Development.json`):

```json
{
  "ConnectionStrings": {
    "DefaultConnection":
      "Host=localhost;Port=5432;Database=wt_lab8;Username=postgres;Password=your_password"
  }
}
```

На Render строка подключения задаётся через переменную окружения
`ConnectionStrings__DefaultConnection` (раздел окружения Web Service), пароль
не хранится в репозитории.

## Миграции

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

После изменения модели (добавление свойства `Description` и т.д.):

```bash
dotnet ef migrations add <имя>
dotnet ef database update
```

## Запуск локально

Требуется .NET 9 SDK и доступная база PostgreSQL (`wt_lab8`).

```bash
dotnet restore
dotnet run
```

Приложение откроется по адресу `http://localhost:10000`.

Основные маршруты:

- `/` — главная;
- `/Cargo/Index` — каталог;
- `/Cargo/Create` — добавление;
- `/Cargo/Details?id=1` — подробная карточка;
- `/Cargo/Edit?id=1` — редактирование;
- `/Cargo/Delete?id=1` — удаление.

## Docker

Сборка образа:

```bash
docker build -t wt-lab8-8razor-postgresql .
```

Запуск контейнера:

```bash
docker run --rm -p 10000:10000 -e PORT=10000 wt-lab8-8razor-postgresql
```

Приложение будет доступно по адресу `http://localhost:10000`.
`Program.cs` читает переменную окружения `PORT` и запускает сервер на
`http://0.0.0.0:{port}`, как требуется для Render.

## Публикация на Render

1. Создать **New → Web Service**.
2. Подключить репозиторий `Balur77/wt-lab8-8razor-postgresql-yushkevich`.
3. Указать:
   - Runtime: **Docker**;
   - Branch: **main**;
   - Dockerfile Path: `./Dockerfile`.
4. Добавить переменную окружения `ConnectionStrings__DefaultConnection` со
   строкой подключения к базе PostgreSQL на Render (пароль не в коде).
5. Запустить deployment и дождаться статуса **Live**.
6. Проверить маршруты `/`, `/Cargo/Index`, `/Cargo/Create`, `/Cargo/Details?id=1`.

Render автоматически передаёт переменную `PORT`, поэтому приложение слушает
внешний порт без дополнительной настройки. Миграции применяются при старте
приложения (`db.Database.Migrate()`).

## Отчёт по лабораторной работе

Готовый PDF-отчёт оформляется по методичке WT-8: цель работы, структура
проекта, модель `Cargo`, `AppDbContext`, параметры подключения к PostgreSQL
(без пароля), миграция `InitialCreate`, скриншоты таблицы
`Products`/`Cargoes` и `__EFMigrationsHistory`, скриншоты работы CRUD и поиска,
ссылки на GitHub и опубликованное приложение на Render.
