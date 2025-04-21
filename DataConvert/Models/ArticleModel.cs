using System.ComponentModel.DataAnnotations;


public class ArticleModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Заголовок обязателен")]
    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    [Required(ErrorMessage = "Текст обязателен")]
    public string? Content { get; set; }

    [Url(ErrorMessage = "Введите корректный URL")]
    public string? ImageUrl { get; set; }

    public string? SourceName { get; set; }

    [Required(ErrorMessage = "Рейтинг обязателен")]
    [Range(-5, 5, ErrorMessage = "Рейтинг должен быть в диапазоне от -5 до 5")]
    public double? PositivityRate { get; set; }

}
