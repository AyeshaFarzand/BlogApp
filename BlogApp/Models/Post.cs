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

        // ✅ Foreign Key for User
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        // ✅ Post Status (Pending, Approved, Rejected)
        [Required]
        public PostStatus Status { get; set; } = PostStatus.Pending;
    }

    // Enum for Post Status
    public enum PostStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
