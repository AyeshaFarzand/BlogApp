using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using BCrypt.Net;

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

        internal static object FindFirst(string nameIdentifier)
        {
            throw new NotImplementedException();
        }

        // Method to hash password
        public void SetPassword(string password)
        {
            string PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            Password = PasswordHash;
        }

        // Method to verify password
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public int FailedLoginAttempts { get; set; } = 0;
        public DateTime? LockoutEndTime { get; set; }
    }
}
