using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BlogApp.Models
{
    public class UserRole
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Role Name must be between 3 and 50 characters.")]
        public string RoleName { get; set; }

        // Navigation property for related Users
        public List<User> Users { get; set; } = new List<User>();
    }
}
