using AyseOzgeErkan_ProjectPhase1.Services;
using BLL.DAL;
using Microsoft.AspNetCore.Mvc;

namespace AyseOzgeErkan_ProjectPhase1.Controllers
{
    public class GenresController : Controller
    {
        private readonly GenreService _genreService;

        public GenresController(GenreService genreService)
        {
            _genreService = genreService;
        }

        // GET: Genres
        public async Task<IActionResult> Index()
        {
            var genres = await _genreService.GetAllGenresAsync();
            return View(genres);
        }

        // GET: Genres/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _genreService.AddGenreAsync(genre);  // Ensure _genreService is being called correctly
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while saving genre: {ex.Message}");
                    ModelState.AddModelError("", "An error occurred while saving the genre.");
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation error: {error}");
                }
            }

            return View(genre);
        }


        // GET: Genres/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetGenreByIdAsync(id.Value);
            if (genre == null) return NotFound();

            return View(genre);
        }

        // POST: Genres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Genre genre)
        {
            if (id != genre.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var success = await _genreService.UpdateGenreAsync(genre);
                if (success)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Failed to update genre.");
            }
            return View(genre);
        }

        // GET: Genres/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var genre = await _genreService.GetGenreByIdAsync(id.Value);
            if (genre == null) return NotFound();

            return View(genre);
        }

        // POST: Genres/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _genreService.DeleteGenreAsync(id);
            if (success)
            {
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Failed to delete genre.");
            return RedirectToAction(nameof(Index));
        }
    }
}
