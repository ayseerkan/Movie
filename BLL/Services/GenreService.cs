using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BLL.DAL;

namespace AyseOzgeErkan_ProjectPhase1.Services
{
    public class GenreService
    {
        private readonly AppDbContext _context;

        public GenreService(AppDbContext context)
        {
            _context = context;
        }

        // Get all genres
        public async Task<List<Genre>> GetAllGenresAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        // Get genre by ID
        public async Task<Genre> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.FindAsync(id);
        }

        // Add a new genre
        public async Task<bool> AddGenreAsync(Genre genre)
        {
            try
            {
                Console.WriteLine($"Adding Genre: {genre.Name}");
                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();
                Console.WriteLine("Genre added successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding genre: {ex.Message}");
                return false;
            }
        }

        // Update an existing genre
        public async Task<bool> UpdateGenreAsync(Genre genre)
        {
            try
            {
                _context.Genres.Update(genre);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete a genre
        public async Task<bool> DeleteGenreAsync(int id)
        {
            try
            {
                var genre = await _context.Genres.FindAsync(id);
                if (genre == null) return false;

                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Check if a genre exists
        public async Task<bool> GenreExistsAsync(int id)
        {
            return await _context.Genres.AnyAsync(e => e.Id == id);
        }
    }
}
