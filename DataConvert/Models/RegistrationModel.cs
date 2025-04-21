using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


public class RegistrationModel
{
    [Required(ErrorMessage = "Required")]
    [EmailAddress]
    [Remote("CheckEmail", "Sample")]
    public string Email { get; set; }
    [Required]
    [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$",
        ErrorMessage = "Пароль должен содержать не менее 8 символов, заглавную и строчную букву, цифру и специальный символ: @$!%*?&")]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirmation { get; set; }

}

