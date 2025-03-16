using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ✅ Store Image Path
        public string? PhotoPath { get; set; }
        // ✅ Post Status (Pending, Approved, Rejected)
        [Required]
        public PostStatus Status { get; set; } = PostStatus.Pending;
        public int UserId { get; set; }
        // ✅ Foreign Key for User
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public ICollection<Like>? Likes { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }

    // Enum for Post Status
    public enum PostStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
