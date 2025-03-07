using System;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class Report
    {
        public int Id { get; set; }

        [Required]
        public string Reason { get; set; }

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}
