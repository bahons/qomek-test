using System.ComponentModel.DataAnnotations;
namespace auth.Models;

/// <summary>
/// Модель регистрации
/// </summary>
public class RegisterModel
{
    [Required]
    public string Login { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    [Compare("Password", ErrorMessage = "Пароли не совпадают")]
    public string ConfirmPassword { get; set; } = null!;
}
