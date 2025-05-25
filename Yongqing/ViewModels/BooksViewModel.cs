using System.ComponentModel.DataAnnotations;

namespace Yongqing.ViewModels;
public class BooksViewModel
{
    [Required]
    public int Id { get; set; }

    [Required(ErrorMessage = "書名為必填")]
    [StringLength(100, ErrorMessage = "書名長度不可超過 100 字")]
    [Display(Name = "書名")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "作者為必填")]
    [StringLength(50, ErrorMessage = "作者名稱不可超過 50 字")]
    [Display(Name = "作者")]
    public string Author { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    [Display(Name = "出版日期")]
    public DateTime? PublishDate { get; set; }

    [Range(0, 10000, ErrorMessage = "價格必須在 0 ~ 10000 之間")]
    [DataType(DataType.Currency)]
    [Display(Name = "價格")]
    public decimal? Price { get; set; }
}
