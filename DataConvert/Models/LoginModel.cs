
using System.ComponentModel.DataAnnotations;

public class LoginModel
{
    [Required(ErrorMessage = "Введите Email")]
    [EmailAddress(ErrorMessage ="Некорректный Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Введите пароль")]
    [MinLength(8,ErrorMessage ="Минимум 8 символов") ]
    public string PasswordHash { get; set; }
}

