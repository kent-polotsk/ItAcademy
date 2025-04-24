using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


public class RegisterModel
{

    [Required(ErrorMessage = "Поле обязательно")]
    [EmailAddress(ErrorMessage = "Некорректный Email")]
    public string Email { get; set; }
    
    
    [Required(ErrorMessage = "Введите пароль")]
    //[RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Пароль не отвечает требованиям")]
    public string Password { get; set; }


    [Required(ErrorMessage = "Подтвердите пароль")]
    [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
    public string PasswordConfirm { get; set; }

}

