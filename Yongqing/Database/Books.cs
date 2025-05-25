namespace Yongqing.Database;
public class Books
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateTime? PublishDate { get; set; }
    public decimal? Price { get; set; }
}
