using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BLL.DAL
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name field cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "The Name field can only contain letters and spaces.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Release Date field is required.")]
        [DataType(DataType.Date)]
        public DateTime? ReleaseDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "The Total Revenue must be a positive value.")]
        public decimal TotalRevenue { get; set; }

        [Required(ErrorMessage = "A Director must be selected.")]
        public int DirectorId { get; set; }

        [ValidateNever] // Ignore validation for the navigation property
        public Director Director { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}