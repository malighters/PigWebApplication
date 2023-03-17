using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PigWebApplication.Models;

namespace PigWebApplication.Controllers
{
    public class PigsController : Controller
    {
        private readonly DbpigsContext _context;

        public PigsController(DbpigsContext context)
        {
            _context = context;
        }

        // GET: Pigs
        public async Task<IActionResult> Index(int? id, string? name)
        {
            // var dbpigsContext = _context.Pigs.Include(p => p.Breed);
            if (id == null) return RedirectToAction("Index", "Breeds");
            
            ViewBag.BreedId = id;
            ViewBag.BreedName = name;

            var pigsByBreeds = _context.Pigs.Where(p => p.BreedId == id).Include(p => p.Breed);

            return View(await pigsByBreeds.ToListAsync());
        }

        // GET: Pigs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Pigs == null)
            {
                return NotFound();
            }

            var pig = await _context.Pigs
                .Include(p => p.Breed)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pig == null)
            {
                return NotFound();
            }

            return View(pig);
        }

        // GET: Pigs/Create
        public IActionResult Create(int breedId)
        {
            //ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Id");
            ViewBag.BreedId = breedId;
            ViewBag.BreedName = _context.Breeds.Where(b => b.Id == breedId).FirstOrDefault().Name;
            return View();
        }

        // POST: Pigs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int breedId, [Bind("Id,Gender,BirthDate,BreedId,Note")] Pig pig)
        {
            pig.BreedId = breedId;
            if (ModelState.IsValid)
            {
                _context.Add(pig);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Pigs", new { id = breedId, name = _context.Breeds.Where(b => b.Id == breedId).FirstOrDefault().Name });
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Id", pig.BreedId);
            return RedirectToAction("Index", "Pigs", new { id = breedId, name = _context.Breeds.Where(b => b.Id == breedId).FirstOrDefault().Name });
        }

        // GET: Pigs/Edit/5
        public async Task<IActionResult> Edit(int? id, int breedId)
        {
            ViewBag.Id = id;
            ViewBag.BreedId = breedId;
            if (id == null || _context.Pigs == null)
            {
                return NotFound();
            }

            var pig = await _context.Pigs.FindAsync(id);
            if (pig == null)
            {
                return NotFound();
            }
            //ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Id", pig.BreedId);
            return View(pig);
        }

        // POST: Pigs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int breedId, [Bind("Id,Gender,BirthDate,BreedId,Note")] Pig pig)
        {
            pig.BreedId = breedId;
            if (id != pig.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PigExists(pig.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            //ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Id", pig.BreedId);
            return RedirectToAction(nameof(Index));
        }

        // GET: Pigs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Pigs == null)
            {
                return NotFound();
            }

            var pig = await _context.Pigs
                .Include(p => p.Breed)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pig == null)
            {
                return NotFound();
            }

            return View(pig);
        }

        // POST: Pigs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Pigs == null)
            {
                return Problem("Entity set 'DbpigsContext.Pigs'  is null.");
            }
            var pig = await _context.Pigs.FindAsync(id);
            if (pig != null)
            {
                _context.Pigs.Remove(pig);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PigExists(int id)
        {
          return _context.Pigs.Any(e => e.Id == id);
        }
    }
}
