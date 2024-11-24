using BLL.DAL;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class DirectorService
    {
        private readonly AppDbContext _context;

        public DirectorService(AppDbContext context)
        {
            _context = context;
        }

        // Fetch all directors asynchronously
        public async Task<List<Director>> GetAllDirectorsAsync()
        {
            return await _context.Directors.ToListAsync();
        }

        // Fetch a director by ID asynchronously
        public async Task<Director> GetDirectorByIdAsync(int id)
        {
            return await _context.Directors.FindAsync(id);
        }

        // Add a new director asynchronously
        public async Task AddDirectorAsync(Director director)
        {
            if (director == null)
            {
                // Optionally, throw an exception or return an error message
                throw new ArgumentNullException(nameof(director), "Director cannot be null");
            }

            _context.Directors.Add(director);
            await _context.SaveChangesAsync();
        }

        // Update an existing director asynchronously
        public async Task UpdateDirectorAsync(Director director)
        {
            if (director == null)
            {
                // Optionally, throw an exception or return an error message
                throw new ArgumentNullException(nameof(director), "Director cannot be null");
            }

            // Only update if the director exists
            var existingDirector = await _context.Directors.FindAsync(director.Id);
            if (existingDirector != null)
            {
                _context.Directors.Update(director);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally, handle the case when the director doesn't exist
                throw new KeyNotFoundException($"Director with ID {director.Id} not found.");
            }
        }

        // Delete a director asynchronously by ID
        public async Task DeleteDirectorAsync(int id)
        {
            var director = await _context.Directors.FindAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Optionally, handle the case when the director doesn't exist
                throw new KeyNotFoundException($"Director with ID {id} not found.");
            }
        }
    }
}
