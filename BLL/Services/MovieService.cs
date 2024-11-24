using BLL.DAL;
using Microsoft.EntityFrameworkCore;

public class MovieService
{
    private readonly AppDbContext _context;

    public MovieService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetAllMoviesAsync()
    {
        return await _context.Movies.Include(m => m.Director).Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre).ToListAsync();
    }

    public async Task<Movie> GetMovieByIdAsync(int id)
    {
        return await _context.Movies.Include(m => m.Director).Include(m => m.MovieGenres).ThenInclude(mg => mg.Genre).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddMovieAsync(Movie movie)
    {
        if (movie.ReleaseDate.HasValue)
        {
            movie.ReleaseDate = DateTime.SpecifyKind(movie.ReleaseDate.Value, DateTimeKind.Utc);
        }

        _context.Movies.Add(movie);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateMovieAsync(Movie movie)
    {
        try
        {
            // Fetch the existing movie from the database
            var existingMovie = await _context.Movies
                .Include(m => m.MovieGenres) // Include related data if needed
                .FirstOrDefaultAsync(m => m.Id == movie.Id);

            if (existingMovie == null)
            {
                return false; // Movie not found
            }

            // Update only the fields that should be modified
            existingMovie.Name = movie.Name;
            existingMovie.ReleaseDate = movie.ReleaseDate;
            existingMovie.TotalRevenue = movie.TotalRevenue;
            existingMovie.DirectorId = movie.DirectorId;

            // Optional: Update relationships, e.g., MovieGenres
            // If MovieGenres is being updated, clear and re-add the collection
            if (movie.MovieGenres != null && movie.MovieGenres.Any())
            {
                _context.MovieGenres.RemoveRange(existingMovie.MovieGenres);
                existingMovie.MovieGenres = movie.MovieGenres;
            }

            // Save changes
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating movie: {ex.Message}");
            return false;
        }
    }

    public async Task DeleteMovieAsync(int id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}