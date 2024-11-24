namespace BLL.DAL
{
    public class MovieGenre
    {
        public int MovieId { get; set; }  // Foreign key for Movie
        public Movie Movie { get; set; }  // Navigation property to Movie

        public int GenreId { get; set; }  // Foreign key for Genre
        public Genre Genre { get; set; }  // Navigation property to Genre
    }
}