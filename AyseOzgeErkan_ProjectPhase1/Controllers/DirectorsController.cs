using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BLL.DAL;
using Microsoft.Extensions.Logging;  // Ensure you are using the correct namespace for ILogger

namespace AyseOzgeErkan_ProjectPhase1.Controllers
{
    public class DirectorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DirectorsController> _logger;  // Inject logger into controller

        // Inject the AppDbContext and ILogger services through the constructor
        public DirectorsController(AppDbContext context, ILogger<DirectorsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Directors
        public async Task<IActionResult> Index()
        {
            var directors = await _context.Directors.ToListAsync();  // Get the list of directors from the database
            return View(directors);  // Pass the list of directors to the view
        }

        // GET: Directors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (director == null)
            {
                return NotFound();
            }

            return View(director);
        }

        // GET: Directors/Create
        public IActionResult Create()
        {
            // Pass the redirectTo parameter from the query string if it exists
            ViewData["RedirectTo"] = Request.Query["redirectTo"];
            return View();
        }

        // POST: Directors/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name, Surname, IsRetired")] Director director)
        {
            if (ModelState.IsValid)
            {
                _context.Add(director);  // Add new director to the database
                await _context.SaveChangesAsync();  // Save changes

                return RedirectToAction(nameof(Index));  // Redirect to the Index page to show updated list
            }
            return View(director);  // If validation fails, return the user to the Create page
        }

        // GET: Directors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors.FindAsync(id);
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
                    _context.Update(director);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DirectorExists(director.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        // Log the exception and rethrow
                        _logger.LogError($"Error updating director with ID {director.Id}");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(director);
        }

        // GET: Directors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var director = await _context.Directors
                .FirstOrDefaultAsync(m => m.Id == id);
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
            var director = await _context.Directors.FindAsync(id);
            if (director != null)
            {
                _context.Directors.Remove(director);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DirectorExists(int id)
        {
            return _context.Directors.Any(e => e.Id == id);
        }
    }
}
