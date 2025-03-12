using BlogApp.Models;

public class Report
{
    public int Id { get; set; }
    public string Reason { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }

    public Post Post { get; set; }
    public User User { get; set; }
}
