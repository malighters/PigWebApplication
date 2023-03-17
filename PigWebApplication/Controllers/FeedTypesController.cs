using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PigWebApplication.Models;

namespace PigWebApplication.Controllers
{
    [Authorize(Roles = "admin, worker")]
    public class FeedTypesController : Controller
    {
        private readonly DbpigsContext _context;

        public FeedTypesController(DbpigsContext context)
        {
            _context = context;
        }

        // GET: FeedTypes
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null) return RedirectToAction("Index", "FeedMixtures");
            ViewBag.FeedmixId = id;
            ViewBag.FeedmixName = name;

            var typesByMix = _context.FeedTypes.Where(ft => ft.FeedmixId == id).Include(ft => ft.Feedmix).Include(ft => ft.Breed);
            //var dbpigsContext = _context.FeedTypes.Include(f => f.Breed).Include(f => f.Feedmix);
            return View(await typesByMix.ToListAsync());
        }

        // GET: FeedTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.FeedTypes == null)
            {
                return NotFound();
            }

            var feedType = await _context.FeedTypes
                .Include(f => f.Breed)
                .Include(f => f.Feedmix)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedType == null)
            {
                return NotFound();
            }

            return View(feedType);
        }

        // GET: FeedTypes/Create
        public IActionResult Create(int feedmixId)
        {
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name");
            //ViewData["FeedmixId"] = new SelectList(_context.FeedMixtures, "Id", "Id");

            ViewBag.FeedmixId = feedmixId;
            ViewBag.FeedmixName = _context.FeedMixtures.Where(fm => fm.Id == feedmixId).FirstOrDefault().Name;
            
            return View();
        }

        // POST: FeedTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int feedmixId, [Bind("Id,BreedId,FeedmixId,AgeStart,AgeFinish,QuantityPerBig")] FeedType feedType)
        {
            feedType.FeedmixId = feedmixId;
            if (ModelState.IsValid)
            {
                _context.Add(feedType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "FeedMixtures", new { id = feedmixId, name = _context.FeedMixtures.Where(fm => fm.Id == feedmixId).FirstOrDefault().Name });
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", feedType.BreedId);
            //ViewData["FeedmixId"] = new SelectList(_context.FeedMixtures, "Id", "Id", feedType.FeedmixId);
            return RedirectToAction("Index", "FeedMixtures", new { id = feedmixId, name = _context.FeedMixtures.Where(fm => fm.Id == feedmixId).FirstOrDefault().Name });
        }

        // GET: FeedTypes/Edit/5
        public async Task<IActionResult> Edit(int? id, int feedmixId)
        {
            ViewBag.FeedmixId = feedmixId;
            if (id == null || _context.FeedTypes == null)
            {
                return NotFound();
            }

            var feedType = await _context.FeedTypes.FindAsync(id);
            if (feedType == null)
            {
                return NotFound();
            }
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", feedType.BreedId);
            //ViewData["FeedmixId"] = new SelectList(_context.FeedMixtures, "Id", "Id", feedType.FeedmixId);
            return View(feedType);
        }

        // POST: FeedTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int feedmixId, [Bind("Id,BreedId,FeedmixId,AgeStart,AgeFinish,QuantityPerBig")] FeedType feedType)
        {
            feedType.FeedmixId = feedmixId;
            if (id != feedType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(feedType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FeedTypeExists(feedType.Id))
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
            ViewData["BreedId"] = new SelectList(_context.Breeds, "Id", "Name", feedType.BreedId);
            //ViewData["FeedmixId"] = new SelectList(_context.FeedMixtures, "Id", "Id", feedType.FeedmixId);
            return View(feedType);
        }

        // GET: FeedTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.FeedTypes == null)
            {
                return NotFound();
            }

            var feedType = await _context.FeedTypes
                .Include(f => f.Breed)
                .Include(f => f.Feedmix)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (feedType == null)
            {
                return NotFound();
            }

            return View(feedType);
        }

        // POST: FeedTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.FeedTypes == null)
            {
                return Problem("Entity set 'DbpigsContext.FeedTypes'  is null.");
            }
            var feedType = await _context.FeedTypes.FindAsync(id);
            if (feedType != null)
            {
                _context.FeedTypes.Remove(feedType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FeedTypeExists(int id)
        {
          return _context.FeedTypes.Any(e => e.Id == id);
        }
    }
}
