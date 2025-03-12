using BlogApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Like
{
    public int Id { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
   

    [ForeignKey("PostId")]
    public Post? Post { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
