using System.ComponentModel.DataAnnotations;

namespace BLL.DAL
{
    public class Director
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Surname { get; set; }

        public bool IsRetired { get; set; }

        // Initialize the collection to avoid null reference issues
        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}