using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogApp.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        // Foreign Key
        [Required(ErrorMessage = "Role ID is required")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public UserRole Role { get; set; }

        internal void SetPassword(object password)
        {
            throw new NotImplementedException();
        }

        internal bool VerifyPassword(object password)
        {
            return true;
        }
    }
}
