using System.ComponentModel.DataAnnotations;


public class ArticleModel
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Заголовок обязателен")]
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime CreationDate { get; set; }
    public DateTime? Updated { get; set; }

    [Required(ErrorMessage = "Текст обязателен")]
    public string? Content { get; set; }

    [Url(ErrorMessage = "Введите корректный URL")]
    public string? ImgUrl { get; set; }

    public string? Source { get; set; }

    [Required(ErrorMessage = "Рейтинг обязателен")]
    [Range(-5, 5, ErrorMessage = "Рейтинг должен быть в диапазоне от -5 до 5")]
    public double? Rate { get; set; }

}
