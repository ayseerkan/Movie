using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class Director
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The Name field can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Surname field is required.")]
        [StringLength(100, ErrorMessage = "The Surname field cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The Surname field can only contain letters and spaces.")]
        public string Surname { get; set; }
        
        [Display(Name = "Retired")]
        public bool IsRetired { get; set; }

        // Initialize the collection to avoid null reference issues
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}