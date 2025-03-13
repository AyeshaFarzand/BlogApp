using BlogApp.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class Comment
{
    public int Id { get; set; }
    public string Content { get; set; }
   
    public DateTime CreatedAt { get; set; }
    public int PostId { get; set; }
    public int? ParentCommentId { get; set; } 
    public Comment? ParentComment { get; set; }
    public List<Comment>? Replies { get; set; }

    public List<CommentLike>? Likes { get; set; }
    [ForeignKey("PostId")]
    public Post? Post { get; set; }

    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}




