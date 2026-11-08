using System.ComponentModel.DataAnnotations;

namespace wt_lab8_8razor_postgresql_yushkevich.Models;

/// <summary>
/// Груз на складе — модель предметной области, перенесённая из WT-4.
/// </summary>
public class Cargo
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Введите название груза")]
    [StringLength(100, ErrorMessage = "Название не должно превышать 100 символов")]
    [Display(Name = "Название")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Укажите категорию")]
    [StringLength(60, ErrorMessage = "Категория не должна превышать 60 символов")]
    [Display(Name = "Категория")]
    public string Category { get; set; } = string.Empty;

    [Range(1, 1_000_000, ErrorMessage = "Количество должно быть от 1 до 1 000 000")]
    [Display(Name = "Количество, шт.")]
    public int Quantity { get; set; } = 1;

    [Required(ErrorMessage = "Укажите место хранения")]
    [StringLength(100, ErrorMessage = "Место хранения не должно превышать 100 символов")]
    [Display(Name = "Место хранения")]
    public string Location { get; set; } = string.Empty;

    [Range(0.01, 1_000_000_000, ErrorMessage = "Вес должен быть положительным числом")]
    [Display(Name = "Вес, кг")]
    public decimal Weight { get; set; }

    [Required(ErrorMessage = "Укажите описание груза")]
    [StringLength(300, ErrorMessage = "Описание не должно превышать 300 символов")]
    [Display(Name = "Описание")]
    public string Description { get; set; } = string.Empty;

    [Display(Name = "Доступен на складе")]
    public bool IsAvailable { get; set; } = true;

    /// <summary>Общий вес партии.</summary>
    public decimal TotalWeight => Math.Round(Weight * Quantity, 2);

    /// <summary>Текстовое представление статуса для интерфейса.</summary>
    public string Status => IsAvailable ? "На складе" : "Нет в наличии";
}
