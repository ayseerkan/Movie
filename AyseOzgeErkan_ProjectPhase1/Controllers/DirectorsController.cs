using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BLL.DAL;
using BLL.Services;

namespace AyseOzgeErkan_ProjectPhase1.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly DirectorService _directorService;
        private readonly ILogger<DirectorsController> _logger;

        public DirectorsController(DirectorService directorService, ILogger<DirectorsController> logger)
        {
            _directorService = directorService;
            _logger = logger;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            var directors = await _directorService.GetAllDirectorsAsync();
            return View(directors);
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var director = await GetDirectorOrLogNotFound(id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Directors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname,IsRetired")] Director director)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Check if a director with the same name and surname already exists
                    var existingDirector = await _directorService.GetDirectorByNameAndSurnameAsync(director.Name, director.Surname);
                    if (existingDirector != null)
                    {
                        ModelState.AddModelError("", "A director with the same name and surname already exists.");
                        return View(director);
                    }

                    var result = await _directorService.AddDirectorAsync(director);
                    if (result)
                    {
                        _logger.LogInformation($"Director {director.Name} {director.Surname} added successfully.");
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Failed to add the director.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while adding a new director.");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(director);
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var director = await GetDirectorOrLogNotFound(id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,IsRetired")] Director director)
        {
            if (id != director.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDirector = await _directorService.GetDirectorByIdAsync(id);
                    if (existingDirector == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingDirector.Name = director.Name;
                    existingDirector.Surname = director.Surname;
                    existingDirector.IsRetired = director.IsRetired;

                    // Update in database
                    var result = await _directorService.UpdateDirectorAsync(existingDirector);
                    if (result)
                    {
                        _logger.LogInformation($"Director {director.Name} {director.Surname} updated successfully.");
                        return RedirectToAction(nameof(Index));
                    }

                    ModelState.AddModelError("", "Failed to update the director.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error updating director with ID {id}.");
                    ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                }
            }

            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var director = await GetDirectorOrLogNotFound(id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // POST: Directors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var director = await GetDirectorOrLogNotFound(id);
                if (director == null)
                {
                    return NotFound();
                }

                var result = await _directorService.DeleteDirectorAsync(id);
                if (result)
                {
                    _logger.LogInformation($"Director with ID {id} deleted successfully.");
                    return RedirectToAction(nameof(Index));
                }

                _logger.LogWarning($"Failed to delete director with ID {id}.");
                ModelState.AddModelError("", "Failed to delete the director.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting director with ID {id}.");
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method to fetch a director and log if not found
        private async Task<Director> GetDirectorOrLogNotFound(int id)
        {
            var director = await _directorService.GetDirectorByIdAsync(id);
            if (director == null)
            {
                _logger.LogWarning($"Director with ID {id} not found.");
            }
            return director;
        }
    }
}
