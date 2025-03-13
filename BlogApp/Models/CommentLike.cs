using BlogApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class CommentLike
{
    public int Id { get; set; }
    public int CommentId { get; set; }
    public Comment Comment { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}