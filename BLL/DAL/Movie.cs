using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BLL.DAL
{
    public class Movie
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Movie name is required.")]
        [MaxLength(100, ErrorMessage = "Movie name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Release Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
        public DateTime? ReleaseDate { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Total Revenue must be a positive value.")]
        public decimal TotalRevenue { get; set; }

        [Required(ErrorMessage = "The Director ID field is required.")]
        public int DirectorId { get; set; }

        [ValidateNever] // Ignore validation for the navigation property
        public Director Director { get; set; }
        public ICollection<MovieGenre> MovieGenres { get; set; } = new List<MovieGenre>();
    }
}