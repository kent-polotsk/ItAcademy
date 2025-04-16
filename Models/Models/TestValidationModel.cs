using System.ComponentModel.DataAnnotations;


public class TestValidationModel
{
    [Required(ErrorMessage = "This field required")]
    public int Id { get; set; }

    [Required]
    [RegularExpression("^([0-9a-zA-Z]([\\+\\-_\\.][0-9a-zA-Z]+)*)+\"@(([0-9a-zA-Z][-\\w]*[0-9a-zA-Z]*\\.)+[a-zA-Z0-9]{2,17})$", ErrorMessage = "Email is invalid")]
    public string Email { get; set; }

    [Required]
    [StringLength(256, MinimumLength = 2)]
    public string Title { get; set; }
}

