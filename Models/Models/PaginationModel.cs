using System.ComponentModel.DataAnnotations;

public class PaginationModel
{
    [Range(1, int.MaxValue)]
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

