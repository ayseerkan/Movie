using BLL.DAL;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<Director> GetDirectorByNameAndSurnameAsync(string name, string surname)
        {
            return await _context.Directors
                .FirstOrDefaultAsync(d => d.Name == name && d.Surname == surname);
        }

        // Fetch all directors asynchronously
        public async Task<List<Director>> GetAllDirectorsAsync()
        {
            return await _context.Directors.AsNoTracking().ToListAsync(); // Use AsNoTracking for read-only queries
        }

        // Fetch a director by ID asynchronously
        public async Task<Director> GetDirectorByIdAsync(int id)
        {
            return await _context.Directors
                .AsNoTracking() // Use AsNoTracking for read-only queries
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        // Add a new director asynchronously
        public async Task<bool> AddDirectorAsync(Director director)
        {
            if (director == null)
                throw new ArgumentNullException(nameof(director), "Director cannot be null");

            try
            {
                await _context.Directors.AddAsync(director); // Use AddAsync for asynchronous addition
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or rethrow the exception as needed
                Console.WriteLine($"Error adding director: {ex.Message}");
                return false;
            }
        }

        // Update an existing director asynchronously
        public async Task<bool> UpdateDirectorAsync(Director director)
        {
            if (director == null)
                throw new ArgumentNullException(nameof(director), "Director cannot be null");

            try
            {
                // Fetch the existing entity
                var existingDirector = await _context.Directors.FirstOrDefaultAsync(d => d.Id == director.Id);
                if (existingDirector == null)
                {
                    throw new KeyNotFoundException($"Director with ID {director.Id} not found.");
                }

                // Update only the fields that need to be changed
                existingDirector.Name = director.Name;
                existingDirector.Surname = director.Surname;
                existingDirector.IsRetired = director.IsRetired;

                // Save changes
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error updating director: {ex.Message}");
                // Replace Console.WriteLine with ILogger if available:
                // _logger.LogError(ex, $"Error updating director with ID {director.Id}");
                return false;
            }
        }

        // Delete a director asynchronously by ID
        public async Task<bool> DeleteDirectorAsync(int id)
        {
            try
            {
                var director = await _context.Directors.FindAsync(id);
                if (director == null)
                    throw new KeyNotFoundException($"Director with ID {id} not found.");

                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or rethrow the exception as needed
                Console.WriteLine($"Error deleting director: {ex.Message}");
                return false;
            }
        }
    }
}
