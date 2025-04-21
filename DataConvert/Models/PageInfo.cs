using System.ComponentModel.DataAnnotations;

public class PageInfo
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int DeviceType { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling((decimal)TotalItems / PageSize);
}

