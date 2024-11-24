namespace BLL.DAL
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation property for the many-to-many relationship
        public ICollection<MovieGenre> MovieGenres { get; set; } // Linking to MovieGenre
    }
}