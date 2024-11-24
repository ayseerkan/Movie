using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class Role
    {
        public int Id { get; set; }

        // Required field with max length
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Navigation property for the one-to-many relationship with User
        // Initialize the collection to avoid null reference
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}