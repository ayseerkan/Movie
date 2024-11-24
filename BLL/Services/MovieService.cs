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

    public async Task UpdateMovieAsync(Movie movie)
    {
        _context.Movies.Update(movie);
        await _context.SaveChangesAsync();
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