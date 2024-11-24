using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class User
    {
        public int Id { get; set; }

        // User name with required validation and max length constraint
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; }

        // Password with required validation, consider storing hash, not plain text
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }

        // Flag indicating if the user is active
        public bool IsActive { get; set; }

        // Foreign key for Role (if Role is mandatory)
        public int RoleId { get; set; }
        
        // Navigation property for the Role (a User has one Role)
        public Role Role { get; set; }

        // Optionally, initialize the navigation property
        public User()
        {
            Role = new Role(); // Initialize Role navigation property (optional)
        }
    }
}