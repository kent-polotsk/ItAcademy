using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;


public class RegistrationModel
{
    [Required(ErrorMessage = "Required")]
    [EmailAddress]
    [Remote("CheckEmail", "Sample")]
    public string Email { get; set; }
    [Required]
    [MinLength(8, ErrorMessage = "!")]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password), ErrorMessage = "Passwords don't match")]
    public string PasswordConfirmation { get; set; }

}

