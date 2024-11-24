using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Services;
using BLL.DAL;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace AyseOzgeErkan_ProjectPhase1.Controllers
{
    public class MoviesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly MovieService _movieService;
        private readonly DirectorService _directorService;
        private readonly ILogger<MoviesController> _logger;

        public MoviesController(AppDbContext context, MovieService movieService, DirectorService directorService, ILogger<MoviesController> logger)
        {
            _context = context;
            _movieService = movieService;
            _directorService = directorService;
            _logger = logger;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAllMoviesAsync();

            Console.WriteLine($"Fetched {movies.Count} movies.");
            foreach (var movie in movies)
            {
                Console.WriteLine($"Movie: {movie.Name}, Director: {movie.Director?.Name}");
            }

            return View(movies);
        }

        // GET: Movies/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDirectorsDropdownAsync(); // Populate dropdown for directors
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,ReleaseDate,TotalRevenue,DirectorId")] Movie movie)
        {
            if (movie.ReleaseDate.HasValue)
            {
                movie.ReleaseDate = DateTime.SpecifyKind(movie.ReleaseDate.Value, DateTimeKind.Utc);
            }

            if (ModelState.IsValid)
            {
                await _movieService.AddMovieAsync(movie);
                return RedirectToAction(nameof(Index));
            }

            await PopulateDirectorsDropdownAsync(movie.DirectorId);
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            await PopulateDirectorsDropdownAsync(movie.DirectorId);
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ReleaseDate,TotalRevenue,DirectorId")] Movie movie)
        {
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Fetch the existing movie from the database
                    var existingMovie = await _movieService.GetMovieByIdAsync(id);
                    if (existingMovie == null)
                    {
                        _logger.LogWarning($"Movie with ID {id} not found.");
                        return NotFound();
                    }

                    // Update properties of the existing movie
                    existingMovie.Name = movie.Name;
                    existingMovie.ReleaseDate = movie.ReleaseDate;
                    existingMovie.TotalRevenue = movie.TotalRevenue;
                    existingMovie.DirectorId = movie.DirectorId;

                    // Save changes to the database
                    var result = await _movieService.UpdateMovieAsync(existingMovie);
                    if (result)
                    {
                        _logger.LogInformation($"Successfully updated movie: {existingMovie.Name}");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to update movie: {existingMovie.Name}");
                        ModelState.AddModelError("", "Failed to update the movie.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating movie with ID {id}");
                    ModelState.AddModelError("", "An unexpected error occurred while updating the movie.");
                }
            }

            // Repopulate the directors dropdown for the view
            await PopulateDirectorsDropdownAsync(movie.DirectorId);

            // Return the view with validation errors if any
            return View(movie);
        }



        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _movieService.DeleteMovieAsync(id);
                _logger.LogInformation($"Successfully deleted movie with ID {id}");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting movie with ID {id}");
                ModelState.AddModelError("", "An error occurred while deleting the movie.");
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        // Helper method to populate directors dropdown
        private async Task PopulateDirectorsDropdownAsync(int? selectedDirectorId = null)
        {
            var directors = await _directorService.GetAllDirectorsAsync();
            ViewBag.Directors = new SelectList(directors, "Id", "Name", selectedDirectorId);
        }
    }
}
